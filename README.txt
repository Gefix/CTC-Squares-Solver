Hybrid Minimal Square Tiling Problem Solver

Table of Contents

0. Prerequisites
1. The Optimal Algorithm
1.1. The initial complexity of the problem
1.2. Dividing the problem into smaller less complex sub-problems
1.2.1. The basic concept
1.2.2. Complexity for puzzle heights greater than 2
1.3. Why exactly 15?
1.4. The optimal algorithm implementation
2. The Heuristic Algorithm
2.1.1. Implementation for Height <= 40
2.1.2. Implementation for Height > 40
2.2. Heuristic algorithm improvements
2.3. Stability of the heuristic solution
2.4. Fine-tuning the heuristic solution for the 10 second requirement
3. Additional Optimizations
4. Web Service Wrapper
5. The Main Front End

0. Prerequisites

Building the solution requires:

I. Visual Studio 2013 or newer;
II. The NuGet Addon for visual studio - for fetching the ServiceStack packages;
III. Microsoft .NET Framework 4.6.

If .NET Framework 4.6 cannot be installed or configured with the IDE the projects can be configured to compile against .NET Framework 4.5 instead.

1. The Optimal Algorithm

Located in the SquareSolver library is the SquareSolverOptimal class we will talk about in this section of the documentation.

In this section we will take a look at a dynamic programming approach towards finding (one of) the optimal solution(s) for the square tiling problem, for puzzles with height M <= 15.
We will use N for puzzle width and M for puzzle height.

1.1. The initial complexity of the problem

We could do a breadth- or depth-first search, however, the number of tilings we will have to check will be astronomical.

Let's consider the 2 * 2, 4 * 2 and 6 * 2 rectangle problem (assuming no gaps):

(We will use hexadecimal numbers to represent the Nth square)

For 2 * 2 we have:

1 3   AND   1 1
2 4         1 1

For 4 * 2 we have:

Solutions with 8 squares:
1 3 5 7
2 4 6 8

Solutions with 5 squares:
1 3 5 5        1 3 3 4        1 1 2 4
2 4 5 5        2 3 3 5        1 1 3 5

Solutions with 2 squares:
1 1 2 2
1 1 2 2

For 6 * 2 all possible tilings are:

Solutions with 12 squares:
1 3 5 7 9 B
2 4 6 8 A C

Solutions with 9 squares:
1 3 5 7 9 9        1 3 5 7 7 8        1 3 5 5 6 8        1 3 3 4 6 8        1 1 2 4 6 8
2 4 6 8 9 9        2 4 6 7 7 9        2 4 5 5 7 9        2 3 3 5 7 9        1 1 3 5 7 9

Solutions with 6 squares:
1 3 5 5 6 6        1 3 3 4 6 6        1 3 3 4 4 5        1 1 2 4 6 6        1 1 2 4 4 5        1 1 2 2 3 5
2 4 5 5 6 6        2 3 3 5 6 6        2 3 3 4 4 6        1 1 3 5 6 6        1 1 3 4 4 6        1 1 2 2 4 6

Solutions with 3 squares:
1 1 2 2 3 3
1 1 2 2 3 3

We went from 2 possible tilings for a 2 * 2 square to 5 tilings for a 4 * 2 and 13 tilings for a 6 * 2 rectangle.
Actually, if we compute the number of tilings for any N * 2 rectangle we will find out that the number will match Fib(N + 1) where Fib is function for the Fibonacci sequence.

Using this we can find out that for a 20 * 2 rectangle there are 10,946 possible unique tilings, and
in a 40 * 2 rectangle there are 165,580,141 different tilings.

Since we are looking for the minimal solution, however, there is no need for us to check all of those unique tilings.

1.2. Dividing the problem into smaller less complex sub-problems

Instead of checking all possible tilings, we can look closely at the partial states we can be in while solving a problem and try to find a way for minimizing the number of square tiling we check.

We are going to use Dynamic Programming to reduce the number of tilings we need to check.

1.2.1. The basic concept

First let's divide the N * 2 problem into N smaller problems of the form:
- "Which are the unique partial tilings covering completely an i * 2 rectangle with the smallest number of squares?" for 0 < i <= N

A partial tiling in this context is a tiling that:
1. Fully covers a subset of the problem - from the 1st to the "i"th column all cells are covered by squares;
2. May or may not cover cells beyond the "i"th column, but ONLY with squares that begin at or before the "i"th column;

A partial tiling with covering i = N is a complete tiling (as by definition 1. all cells up to and including the "i"th column must be covered, when i = N that means the whole problem)

As a consequence, all partial tilings covering the first i columns can be derived from all partial tilings covering the first (i-1) columns by adding squares so that the tilings cover i columns fully.

Using this approach, we can compute all optimal partial tilings for each i between 1 and N:
N = 6 (width), M = 2 (height)

i = 1 has two possible partial tilings:
1 # # # # #        1 1 # # # #
2 # # # # #        1 1 # # # #
(2 squares)        (1 square)

i = 2 has, again, two possible partial tilings:
1 3 3 # # #        1 1 # # # #
2 3 3 # # #        1 1 # # # #
(3 squares)        (1 square)

i = 3 seems to have three possible partial tilings:
1 3 3 # # #        1 1 2 # # #        1 1 2 2 # #
2 3 3 # # #        1 1 3 # # #        1 1 2 2 # #

However, we can see that the first two are identical in coverage. By choosing only the one with the smallest number of squares or if they are the same the first we found, we are left with only 2 unique optimal tilings:
1 3 3 # # #        1 1 2 2 # #
2 3 3 # # #        1 1 2 2 # #
(3 squares)        (2 squares)

i = 4 has, again, two possible partial tilings:
1 3 3 4 4 #        1 1 2 2 # #
2 3 3 4 4 #        1 1 2 2 # #
(4 squares)        (2 squares)

i = 5 has, again, two possible partial tilings:
1 3 3 4 4 #        1 1 2 2 3 3
2 3 3 4 4 #        1 1 2 2 3 3
(4 squares)        (3 squares)

And finally, i = 6 has, initially, two possible partial tilings:
1 3 3 4 4 5        1 1 2 2 3 3
2 3 3 4 4 6        1 1 2 2 3 3
(6 squares)        (3 squares)

Since we only store unique solutions, the first tiling will be overwritten by the second due to it having a lower number of squares solution for the exact same coverage.

After computing i = N we will always have only one solution, and that solution will be the optimal one.

A key point in this approach is that, (for height = 2), except for the first 2 columns and the last column, we are checking 3 unique tilings per column, choosing 2 of them and discarding 1.
It may seem like a small reduction, but that single discarded tiling effectively reduces the total number of tilings we need to check from Fib(N + 1) to 3 * (N - 1).
This means that we now need only 3 * (40 - 1) or a total of 117 unique (partial) tilings for the 40 * 2 rectangle without black cells (instead of 165,580,141 which means ~ 1,415,214 times less tilings compared to a breadth- or depth-first approach). For larger N the difference will get even bigger.

Increasing the height, however, will also greatly increase the number of partial tilings we need to check. It will still be much less than the total number of possible tilings but will not be a linear function of N.

1.2.2. Complexity for puzzle heights greater than 2

Let's consider puzzles with height > 2. For any height M < N, if we do not black cells, we can not tile squares of size anywhere between 1 and M.

For example, for M = 3 we will now have 4 unique partial tilings in the first column (i = 1) up from 2. Those tilings would be:
M = 3, i = 1
1 # # # # #        1 # # # # #        1 1 # # # #        1 1 1 # # #
2 # # # # #        2 2 # # # #        1 1 # # # #        1 1 1 # # #
3 # # # # #        2 2 # # # #        2 # # # # #        1 1 1 # # #
(3 squares)        (2 squares)        (2 squares)        (1 square)

For M = 4 we will have 8 unique partial tilings covering the first column (i = 1). Those tilings would be:
M = 4, i = 1
1 # # # # #        1 # # # # #        1 # # # # #        1 # # # # #        1 1 # # # #        1 1 # # # #        1 1 1 # # #        1 1 1 1 # #
2 # # # # #        2 # # # # #        2 2 # # # #        2 2 2 # # #        1 1 # # # #        1 1 # # # #        1 1 1 # # #        1 1 1 1 # #
3 # # # # #        3 3 # # # #        2 2 # # # #        2 2 2 # # #        2 # # # # #        2 2 # # # #        1 1 1 # # #        1 1 1 1 # #
4 # # # # #        3 3 # # # #        3 # # # # #        2 2 2 # # #        3 # # # # #        2 2 # # # #        2 # # # # #        1 1 1 1 # #
(4 squares)        (3 squares)        (3 squares)        (2 square)         (3 squares)        (2 squares)        (2 squares)        (1 square)

The number of tilings covering the 1st column for any M is actually equal to the sum of all partial first-column tilings with heights < M, plus 1. Or, if we simplify it, 2 to the power of (M - 1), or 2 ^ (M - 1). (We will use the ^ operator for 'to the power of')

This number, however, is only for the partial tilings covering the first column. When we increase "i" we will notice that we now can have more partial tilings. The reason for that is that instead of starting off from an invisible vertical wall of black cells (i = 0), we will have partial tilings that are based on many different previous partial tilings. For M = 4, i = 2 we will have a total of 16 derived partial tilings.

We will define two upper boundaries:
a) The maximum number of unique tilings covering i columns for i > 1
b) The maximum number of derived partial tilings covering i columns from the tilings covering i - 1 columns;

The number of unique tilings comes down the shape of the right boundary of the partial tiling. Each unique possible right boundary shape can be made with one or more partial tilings. Since we are looking at shapes that cover i columns completely, we can ignore the first i columns and only focus on what shapes can we have after that. As a consequence of this, we can have shapes where it would seem that we have tiles a rectangle, while in fact this would just be what's left of a square we've put at a previous X position.

Again, using dynamic programming with memoization, we can find out the number of such unique possible shapes.
The same approach we can apply for the number of derived partial tilings.

In the SquaresSolver project the SquareTilingCombinatorics class contains methods for computing both the UniqueTilings and the DerivedTilings for any given height.

The numbers are as follows:

Height ,  UniqueTilings*,     DerivedTilings*
     1 ,              1 ,                  1
     2 ,              2 ,                  2
     3 ,              5 ,                  6
     4 ,             11 ,                 16
     5 ,             24 ,                 42
     6 ,             53 ,                112
     7 ,            118 ,                299
     8 ,            261 ,                796
     9 ,            577 ,               2119
    10 ,           1276 ,               5643
    11 ,           2823 ,              15027
    12 ,           6246 ,              40016
    13 ,          13819 ,             106561
    14 ,          30572 ,             283766
    15 ,          67635 ,             755652
...
    40 ,  28275142888920,  32535991991782479


* per iteration

What that means is that, we will have at most 67635 unique states when tiling an N * 15 rectangle from left to right, one column at a time. And on each iteration, we can expect to have no more than 755652 derived tilings, which we need to assess and merge, again, into at most 67635 unique states for the next iteration.

By going iteratively from i = 1 to i = N we will need to check at most a total of DerivedTilings(M) * N partial tilings. The key part here is the deduplication - without filtering out the derived tilings into a set of unique shapes wi would have to check even more tilings on next iteration. 

For N * M, the total amount of complete (not partial) tilings possible is ~ < UniqueTilings(M) * ( ( DerivedTilings(M) / UniqueTilings(M) ) ^ N )

That means that for a 15 * 15 puzzle, we would have approximately less than 356801458571554192170 possible tilings, out of which we will only check at most UniqueTilings(M) * 15 or 11334780 partial tilings, before finding out the optimal solution of the puzzle. This is a 31478463505383 times reduction. And for 40 * 15 the improvement would be 1,8871366e+39.

1.3. Why exactly 15?

The act of de-duplicating the derived partial tilings requires that we have means for comparing their coverage.
The need for comparing their coverage necessitates that for each partial tilings we have a state describing its right boundary shape.

If, for example, midway through an iteration, we have found 30000 uniquely shaped tilings and need to find out whether the next derived tiling matches in shape with any of those - we should be able to do that as quickly as possible. We could iterate linearly through all existing unique partial tilings for that level but that would make the search extremely slow.

The state is, in our case, an array of M numbers, where M is the height of the puzzle. Each element in the array tells how many blocks to the right does the partial tiling protrude from the column we are processing (including). So, if we chose to tile the first column with squares of size 1 - the state of that partial tiling will be all 1s. If we chose to tile the first column with a square the size of the height of the puzzle - the state will be M times the number M (M - height).

When we move to the next iteration we copy all states and then subtract 1 from all their numbers - this is the shape we will be working on for finding out derived partial tilings.

E.g. in the 6 * 2 example we had two possible tilings for i = 1: resulting in states of all 1s and all 2s. On the next iteration, i = 2, the first state would get transformed into all 0s meaning we need to put some new squares to cover the current column. The second state will, however, become all 1s - all cells on the second column will be covered, so there would be nothing more to add - we just pass the state, and as a result the partial tiling, through to the next iteration.

If we add a square of size X, it would take X iterations before the numbers corresponding to the part of the state associated with that square become 0 - that's when we would be able to add more squares to the tiling - until then we can only work around that square.

So, having this in mind - the minimum number we can have in our state is 0, which would mean no coverage for the cell in the current column we're looking at.
The maximum number would be M in the case when there are no black spots.
And the number of elements in the state is also M - one for each Y coordinate.

We can use a number to uniquely identify each coverage shape. 
There are two ways for finding such a number:
a) we can use the index of the combination if we order all shapes lexicographically in a certain order;
b) if the state is small enough to fit in we could use it directly

We can compute a) using the CombinationsBefore method from the SquareTilingCombinatorics class, but this would require in the worst case M calculations.

Instead, we can find something interesting about the M = 15 case:

All the numbers in all the states will always be in the range 0 - 15!

This means that they require only 4 bits to be stored, and we can store 15 4-bit numbers in 60 bits.

If our state is one 64-bit long integer we do not have to do any calculations to find a number corresponding to it - it already is that number!

In addition to that, storing the whole state in one 64-bit integer means when we need to clone the state we only need to perform one assignment for transferring the whole state to another object.

1.4. The optimal algorithm implementation

The SquareTilingOptimal class from the SquareSolver project implements the algorithm described herein.

The implementation uses the PathStateShort class for representing partial states. That class contains:
I. The cost - the number of all squares in the partial tiling so far;
II. The state - a single 64-bit long integer;
III. A reference to the state from the (i - 1)th column from which the current state was derived. This is needed if we are to retrace our path from the end of the solution all the way to its beginning and find out what squares we used when. Without this we will only be able to answer what is the minimal number of squares as a number - not knowing what squares we need to put where.

The SquareTilingOptimal class derives from the SquareTilingBase and works as follows:

We initialize an instance of the class with the puzzle we are trying to solve by passing it to the constructor.
We then call the public method Solve to get the optimal solution.
Solve calls CalculateMaxSquares to find out the maximum squares we can put at any X, Y coordinates within the puzzle. Also computed using a dynamic programming technique this gives us an upper boundary for the square we can put at each cell.
We initialize the i = 0 state with all 0s and push it as the only starting partial tiling.
We have two sets (one list and one dictionary): left and right
	left represents the unique partial tilings we have found prior to the current column we are working on,
	right is the de-duplicated set of derived tilings that fully cover the column we are working on.
Then, for each column we:
	Cycle through all partial tilings from the previous column,
	Clone and subtract 1 from each state, assigning the original state to be the previous state of the cloned one,
	Compute the MaxSize and JumpTo memoizations that we'll need next,
	Call Tile which will do a depth first recursive search for finding all possible derived tilings from this one, and store them in a Dictionary hash map (for faster de-duplication lookups),
	Replace our left set with the values from the right set and clear the right set.
At the end we will have a single solution in the left set. That solution will be the optimal one. We then call the GetSolution method to convert the history of states associated with it into a list of squares and return the result.
	
The MaxSize and JumpTo arrays serve to optimize the number of checks required while looking for possible derived tilings.
The way they work is as follows:
Let's say we have the following left partial tiling:

1 # # # # #
2 @ # # # #
3 3 # # # #
3 3 # # # #
4 # # # # #
5 # # # # #

( @ means a black cell where we cannot put squares )

This will be represented by the following state:

[ 1, 1, 2, 2, 1, 1 ]

When we move to the next column, the state will change to:

[ 0, 0, 1, 1, 0, 0 ]

This means we now have 4 potentially available cells we should put square(s) into.

The MaxSize array contains the largest size square we can put at a given Y coordinate, for the column (X) we are currently working on.
Since the second cell of the second column is not available we cannot put a 2 * 2 square at the top.
As a result the MaxSize array for i = 2 of this partial tiling will look like:

[ 1, 0, 0, 0, 2, 1 ]

Notice how when we have a square that's still "in the making" we also consider the space it occupies as unavailable from tiling's perspective.

The JumpTo is a secondary optimization that the Tile recursion uses, to quickly skip over any sequence of unavailable cells - no need to check each cell if it's available or not.
The JumpTo array for the above example will look like:

[ 0, 3, 2, 1, 0, 0 ]

Notice how when we can put any tile the value is 0, and when we cannot the value is the number of cells we need to skip to go to the first available cell we can tile a square into.

As a result, the logic of the Tile function can now be significantly simplified.

2. The Heuristic Algorithm

Located in the SquareSolver library are the SquareTilingHeuristic and SquareTilingHeuristicLarge classes we will talk about in this section of the documentation.

The optimal algorithm works fast enough for puzzle heights up to 15, but becomes exponentially slower for higher values.

For instance, for a puzzle with dimensions of 40 * 40 we will have upper bounds of 28275142888920 unique puzzles and 32535991991782479 derived puzzles. If the memory needed by the state of one puzzle was e.g. 40 bytes, that results in over 1028 TiBs of data for one single iteration - we would need to store up to 40 times that, or over 40 PiBs - even if we had that amount of storage, storing that data in under 10 seconds would be an engineering feet like no other. Even the CERN Data Centre can only process 10 GiBs per second with its 100,000 cores! And we are talking about 4 million times that amount of data.

What we can do instead is we can limit the number of partial solutions we look at at each column / iteration.

We do this by introducing two limits to our Solver and partial Tiler:
I. Cost Margin - the maximum difference between the minimal cost of a derived partial tiling we have found so far in the iteration and the partial tiling we are currently looking at. If for example we find a partial covering solution with Z squares, and after that we start working on a derived partial solution which will lead to a more than (Z + Cost Margin) squares solution - we will not even consider checking it out. We will ignore it, not store it and thus decrease the number of both derived partial tilings and as a consequence the unique partial tilings.

II. Size Deviation Margin - the maximum amount of cells we will consider "shrinking" a square. What this means is that if for example we can put in a given tiling a square the size of 10, and our Size Deviation Margin is 2, we will only check solutions in which we tile a square with sizes 10, 9 and 8. There is just one special case - in the case where we have not found a single other partial solution yet, we will continue trying to tile with a smaller square so that we have at least 1 solution on the right side for every iteration.

By applying this heuristic, we will still be checking out a lot of possible solutions, but they will in most cases be solutions with square numbers close to the minimum. We can, of course, dismiss checking out a potentially better solution that just happened to have a higher number of squares in one of the iterations and was dropped because of that. That's why this is a heuristic and not an optimal solver.

Even with these limitations in place, for example a configuration of 
Cost Margin = 4 and size Deviation = 2 can have over 100,000 unique partial tilings per iteration and even close to 500,000 depending on the complexity of the puzzle.

Choosing a Cost Margin of 0 significantly reduce the number of partial unique tilings, and using a Size Deviation of 0 will mean that we will just pick the first solution we can find - a Greedy solution rather than Heuristic. In both cases the algorithm will run in near linear time.

2.1. Heuristic algorithm optimizations

Since we are not dropping potential partial tilings, we can pre-compute the minimum number of squares needed to finish tiling the "i"th column for a particular starting state.
Similarly to the MaxSize and JumpTo memoizations, we add a MinSquares memoization that tells us, for each left state we are looking at - how many squares do we need minimum if we need to cover completely the current column starting from row Y.
In the example above, the MinSquares array would be:

[ 2, ?, ?, ?, 1, 1 ]

Notice the ?s - that's because we do not need to know the values for these positions - hence we do not compute them. Similarly, we will not compute all values of the JumpTo and MaxSize memoizations - only the once that the Tile recursive function will actually need.

By doing this MinSquares memoization we can now determine at any point during our recursive tiling whether we will reach a solution that would be worse than the best partial solution found so far + Cost Margin. And if we do - we will not go deeper down the depth-first recursion.

There is something else, however, that we need to figure out:

We cannot use the state compaction we used in the <= 15 optimal solution.

But we can use another one. Or even two.

We will consider two storage solutions for our states.

2.1.1. Implementation for Height <= 40

If the height of the solution is <= 40, we can store the state in 4 * 64-bit long integers. Here's how:

The state numbers will then vary from 0 to 40, which is < 64, so 6 bits will be more than enough to store an element of the state.
We can store 10 state elements in one 64-bit long integer, and we'll still have 4 bits left - unfortunately, we cannot easily utilize the left-overs as the low-level arithmetic then cannot be optimized as much. So 10 elements is the maximum for one long integer.

Therefore, in 4 such long integers we can store the state of any puzzle with height up to 40.

There's now another issue though - we can no longer use one 64-bit integer as the key with which to de-duplicate our results. Having a 256-bit key for a Dictionary (hash map) is much slower than having a 64-bit key.
However, we also know that even if we do all possible tilings we will get no more than 28275142888920 unique tilings per iteration. That is less than 2 ^ 45. This means that we can use a 64-bit integer to uniquely identify each partial coverage / tiling. We do that with the help of the CombinationsBefore combinatorics method. Remember - we calculate this number based on the actual coverage of the partial tiling, not on what squares we have put in it - it's the border that we care about being unique anyway.

2.1.2. Implementation for Height > 40

If we do want to solve puzzles with height greater than 40 there is still a way:

We just change from PathStateMedium (the M <= 40 version) to PathStateLarge - a state storage class which uses a plain integer array internally to store the state.
Doing that, however, will mean that we will not be able to rely on the CombinationsBefore method anymore - as it's optimized for Heights <= 40.
Instead, we can compute the hash of the 32-bit array using the Fnv1a64 hashing algorithm - a fast hashing algorithm, which we modified to work on arrays of 32-bit integers, and which returns again a 64-bit hash we can use for de-duplication purposes.

This means that some partial tilings which have different coverages can in theory collide with the same hash value - we'll just use the one with the lower number of squares, and ignore the other - since this is a heuristic algorithm anyway that loss of information is acceptable.

2.2. Heuristic algorithm improvements

After we have completed a heuristic search for a solution and identified a potential candidate. We can do a subsequent improvement on the result.

The improvement consists of running the optimal tiling algorithm on a subset of the original puzzle with the following twist:

We pick a sub-rectangle of height 15, we remove all squares from the heuristic solution that fall completely in the sub-rectangle, we then create new SquareTilingOptimal instance and give it a puzzle in which any cells that fall into squares reaching outside the sub-rectangle are marked as black - unavailable.
We then run the optimal solver.
If the solution from the solver contains less squares than the squares we removed from the sub-rectangle region of the heuristic solution - we put it in place of the removed squares.

This technique we do for 4 different sets of stripes:
I. Horizontal stripes of height 15, starting from Y = 0, and going down with a fixed step (of 10 cells)
II. Vertical stripes of *width* 15, starting from X = 0, and going right with a fixed step (of 10 cells)
III. Horizontal stripes of height 15, starting from Y = M - 15, and going up with a fixed step (of 10 cells)
IV. Vertical stripes of *width* 15, starting from X = N - 15, and going left with a fixed step (of 10 cells)

2.3. Stability of the heuristic solution

The heuristic algorithm will give good results, however, they can be different depending on which direction do we start tiling from.
To minimize that effect we are going to run 8 heuristic tiling instances in parallel:

We do not do that in the heuristic implementation itself, but rather in the front-end of the puzzle solver:

I. The original puzzle we received - going left to right, top down within each column;
II. The X- inverse of the original puzzle;
III. The Y- inverse of the original puzzle;
IV. The XY- inverse of the original puzzle;
V. The transposed matrix of the original puzzle;
VI. The X- inversed transposed matrix of the original puzzle;
VII. The Y- inversed transposed matrix of the original puzzle;
VIII. The XY- inversed transposed matrix of the original puzzle;

2.4. Fine-tuning the heuristic solution for the 10 second requirement

To make sure the solver will always try to return some result in less than 10 seconds even if it's not optimal the following limits have been implemented:
I. If during the heuristic phase we exceed the 4000 milliseconds mark before reaching the 20th iteration (the 20th column), we reduce the Cost Margin to 3;
II. If during the heuristic phase we exceed the 6000 milliseconds mark, we reduce the Cost Margin to 2;
III. If during the heuristic phase we exceed the 7000 milliseconds mark, we reduce the Cost Margin to 0;
IV. If during the optimal sub-rectangle optimization phase of the heuristic implementation we exceed the 8500 milliseconds mark, we stop checking other stripes and effectively return what we have obtained so far.

These numbers can and should be externalized and made configurable - to make the algorithm flexible and allow it to run in other time frames than 10 seconds only - this is just a work-in-progress.

The 8500 is needed, since, on average, it can take around 500 milliseconds for the optimal solver to find a solution - this means we can return in up to 8500 + 500 milliseconds or 9 seconds. In addition to that, there is the added overhead of the SolverService REST API (de/serialization of the puzzle request/response, routing logic within the framework etc.).
Add to that 50ms for reaction time before we decide we will no longer wait for results from the services, add 2 * 200ms network latency time when we contact the Tech Challenge solver server from Bulgaria - 200ms for the puzzle GET request roundtrip and 200ms for the puzzle solution POST roundtrip - and we reach 9950ms - just under 10 seconds.

3. Additional Optimizations

Since we are using the generic C# Dictionary<T,T> container a lot we have made some optimization improvements to it - by removing some of the internal checks present within it, and marking the most frequently used methods for aggressive inlining we have achieved a ~ 10% to 20% improvement on the performance of the container. First we reverse-engineered the code from the .NET library using ILSpy, then we made the modifications, included all necessary internal classes so that it will compile, and linked it with out binary. We can easily switch to the original Dictionary implementation simply by changing the FastDictionary.cs file to not get compiled.

We have also marked for aggressive inlining the methods used for retrieving and storing state elements in the PathState* classes, and on other places where deemed fit. This will not necessarily mean that the methods will get inlined, it's just a hint.

Also, we get an additional improvement when we compile the code in 64-bit mode and execute it on a 64-bit hardware.
The .NET Framework 4.6 has the best performance for 64-bit applications, but its default 64-bit JIT compiler - the new RyuJIT - is still in beta and gives poor results. Instead, we have configured our apps to use the legacy JIT compiler. This gives slightly better results than the .NET 4.5 version of the 64-bit compiler.

Always build in Release mode when executing the solver in production.

4. Web Service Wrapper

If we have an 8-core processor we can easily run 1 solver on each core and theoretically compute all 8 derived puzzles in the same time we can compute one puzzle.
That would be, if the garbage collector of the .NET managed environment was not single-threaded by nature. And since most of what we do in the algorithm is allocate a lot of state objects and then forget about them - all those actions go through the managed memory manager, flooding it with work, and resulting in somewhere around 50% CPU utilization instead of the 100% we expect to get.

The solution to that is simple - use multiple processes.

The implementation is also simple - we use the ServiceStack Web Service framework to create a simple REST (also supports SOAP) service, that listens for work requests and executes them using the most appropriate tiling for the puzzle we give it.

The Service is split into three project:

a) SquaresService - the actual service implementation that relays the work to the appropriate solver;
b) SquaresServiceInterface - containing just the DTOs we need to know if we are to communicate with the SquareService service;
c) SquaresServiceTypes - containing the Square class - a DTO that is used by both the service and by the actual square solver algorithms - this is needed if we want to understand the result returned from the service DTOs. Technically this is not part of the service but of the solver.

The service is compiled as a Console Application containing a self-hosted ServiceStack service in it. Via a command-line argument we can control which port it will run on. Currently we can run up to 10 instances of the service on ports 8900 to 8909 - if needed this can be easily modified to include more ports. If we do not supply an argument the default port will be 1330 - to avoid conflicts when running the service from the IDE for debug purposes.

5. The Main Front End

The SquaresFrontEnd project contains the glue that connects the solvers and the services with the Tech Challenge puzzle server.

The GUI consists of a single form - the SquareSolverForm class - that has logic for performing the following tasks:

We can enter the number of puzzles we want to solve on the online puzzle server, select the mode - contest or trial, and click the Solve Online Puzzles button.

This will start a thread, which will fire up 9 instances of our SolverService.

The first 8 instances are for the heuristic algorithm with a Cost Margin of 4.
The 9th instance is used for getting a solution fast - with a Cost Margin of 0.

Both use solution improvements afterwards as per 2.2.

After we create all 9 services, we create 9 threads, in each thread we connect to one of the services, give the respective puzzle request and wait for the result.

Every 50ms we check how many threads have completed the request, and select the optimal solution amongst all received solutions.

After 9.5 seconds we return the best solution we have found so far, if a thread has not yet completed its wait for a response from the solver service we will not take its result into consideration - it's more important to send a result to the Tech Challenge contest server even if we have not completed all sub-tasks yet.

Once we send our solution, we enter the clean-up phase.

We wait for all threads to complete executing their requests - in case we have any late threads.
We then issue a CleanupService request on all running services. This will force garbage collection ensuring all the memory used by the solver algorithm is freed and the heap compacted so that we do not run out of memory in our subsequent puzzle solving requests.

After we have executed the specified by the user number of online square tiling puzzles, we terminate our services and return from all threads.

When executing that batch solver we will always first execute 1 trial puzzle request - to warm up the services and Json (de)serializers - and then the actual number of puzzles for the user-selected mode.

After each puzzle solving is done the GUI will display a graphical representation of the puzzle with the squares tiling of the best found solution, the number of squares, the number of results captured from all services prior to sending a response to the online contest server, and the puzzle matrix manipulation we have performed prior to running the solver that has resulted in this solution. E.g. if we find out that after transposing and inverting the Y coordinates of the input puzzle the heuristic solution will give a better solution than the rest - that solution will be displayed as if we never transposed or inverted the Y, but the Transpose and Inverse Y checkboxes will be checked.

The result of each puzzle task will be shown in the listbox log, logged to file, and also the json of the actual input puzzle saved to disk in a configurable folder.

If we change the algorithm and want to re-run and see what results we will get for an old puzzle we can click the "Re-Solve Local Puzzle" button, which will let use select a json file for our puzzle and run the same parallel solving technique on it.

