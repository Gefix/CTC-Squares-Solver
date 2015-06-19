using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Threading;

namespace SquaresSolver
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal sealed class __DynamicallyInvokableAttribute : Attribute
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public __DynamicallyInvokableAttribute()
        {
        }
    }

    /// <summary>Represents a collection of keys and values.</summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <filterpriority>1</filterpriority>
    [__DynamicallyInvokable, ComVisible(false)]
    [Serializable]
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ISerializable, IDeserializationCallback
    {
        private struct Entry
        {
            public int hashCode;
            public int next;
            public TKey key;
            public TValue value;
        }
        /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        [__DynamicallyInvokable]
        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
        {
            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;
            private Dictionary<TKey, TValue> dictionary;
            private int version;
            private int index;
            private KeyValuePair<TKey, TValue> current;
            private int getEnumeratorRetType;
            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2" /> at the current position of the enumerator.</returns>
            [__DynamicallyInvokable]
            public KeyValuePair<TKey, TValue> Current
            {
                [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
                get
                {
                    return this.current;
                }
            }
            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the collection at the current position of the enumerator, as an <see cref="T:System.Object" />.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
            [__DynamicallyInvokable]
            object IEnumerator.Current
            {
                [__DynamicallyInvokable]
                get
                {
                    if (this.index == 0 || this.index == this.dictionary.count + 1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }
                    if (this.getEnumeratorRetType == 1)
                    {
                        return new DictionaryEntry(this.current.Key, this.current.Value);
                    }
                    return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
                }
            }
            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the dictionary at the current position of the enumerator, as a <see cref="T:System.Collections.DictionaryEntry" />.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
            [__DynamicallyInvokable]
            DictionaryEntry IDictionaryEnumerator.Entry
            {
                [__DynamicallyInvokable]
                get
                {
                    if (this.index == 0 || this.index == this.dictionary.count + 1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }
                    return new DictionaryEntry(this.current.Key, this.current.Value);
                }
            }
            /// <summary>Gets the key of the element at the current position of the enumerator.</summary>
            /// <returns>The key of the element in the dictionary at the current position of the enumerator.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
            [__DynamicallyInvokable]
            object IDictionaryEnumerator.Key
            {
                [__DynamicallyInvokable]
                get
                {
                    if (this.index == 0 || this.index == this.dictionary.count + 1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }
                    return this.current.Key;
                }
            }
            /// <summary>Gets the value of the element at the current position of the enumerator.</summary>
            /// <returns>The value of the element in the dictionary at the current position of the enumerator.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
            [__DynamicallyInvokable]
            object IDictionaryEnumerator.Value
            {
                [__DynamicallyInvokable]
                get
                {
                    if (this.index == 0 || this.index == this.dictionary.count + 1)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }
                    return this.current.Value;
                }
            }
            internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
            {
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.current = default(KeyValuePair<TKey, TValue>);
            }
            /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            [__DynamicallyInvokable]
            public bool MoveNext()
            {
                if (this.version != this.dictionary.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                while (this.index < this.dictionary.count)
                {
                    if (this.dictionary.entries[this.index].hashCode >= 0)
                    {
                        this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
                        this.index++;
                        return true;
                    }
                    this.index++;
                }
                this.index = this.dictionary.count + 1;
                this.current = default(KeyValuePair<TKey, TValue>);
                return false;
            }
            /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" />.</summary>
            [__DynamicallyInvokable]
            public void Dispose()
            {
            }
            /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            [__DynamicallyInvokable]
            void IEnumerator.Reset()
            {
                if (this.version != this.dictionary.version)
                {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
                this.index = 0;
                this.current = default(KeyValuePair<TKey, TValue>);
            }
        }
        /// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited.</summary>
        [__DynamicallyInvokable]
        [Serializable]
        public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
            [__DynamicallyInvokable]
            [Serializable]
            public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
            {
                private Dictionary<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TKey currentKey;
                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> at the current position of the enumerator.</returns>
                [__DynamicallyInvokable]
                public TKey Current
                {
                    [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
                    get
                    {
                        return this.currentKey;
                    }
                }
                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the collection at the current position of the enumerator.</returns>
                /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
                [__DynamicallyInvokable]
                object IEnumerator.Current
                {
                    [__DynamicallyInvokable]
                    get
                    {
                        if (this.index == 0 || this.index == this.dictionary.count + 1)
                        {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                        }
                        return this.currentKey;
                    }
                }
                internal Enumerator(Dictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;
                    this.index = 0;
                    this.currentKey = default(TKey);
                }
                /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" />.</summary>
                [__DynamicallyInvokable]
                public void Dispose()
                {
                }
                /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
                /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                [__DynamicallyInvokable]
                public bool MoveNext()
                {
                    if (this.version != this.dictionary.version)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
                    while (this.index < this.dictionary.count)
                    {
                        if (this.dictionary.entries[this.index].hashCode >= 0)
                        {
                            this.currentKey = this.dictionary.entries[this.index].key;
                            this.index++;
                            return true;
                        }
                        this.index++;
                    }
                    this.index = this.dictionary.count + 1;
                    this.currentKey = default(TKey);
                    return false;
                }
                /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                [__DynamicallyInvokable]
                void IEnumerator.Reset()
                {
                    if (this.version != this.dictionary.version)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
                    this.index = 0;
                    this.currentKey = default(TKey);
                }
            }
            private Dictionary<TKey, TValue> dictionary;
            /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.Retrieving the value of this property is an O(1) operation.</returns>
            [__DynamicallyInvokable]
            public int Count
            {
                [__DynamicallyInvokable]
                get
                {
                    return this.dictionary.Count;
                }
            }
            [__DynamicallyInvokable]
            bool ICollection<TKey>.IsReadOnly
            {
                [__DynamicallyInvokable]
                get
                {
                    return true;
                }
            }
            /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
            /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns false.</returns>
            [__DynamicallyInvokable]
            bool ICollection.IsSynchronized
            {
                [__DynamicallyInvokable]
                get
                {
                    return false;
                }
            }
            /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
            /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns the current instance.</returns>
            [__DynamicallyInvokable]
            object ICollection.SyncRoot
            {
                [__DynamicallyInvokable]
                get
                {
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }
            /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
            /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose keys are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="dictionary" /> is null.</exception>
            [__DynamicallyInvokable]
            public KeyCollection(Dictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
                }
                this.dictionary = dictionary;
            }
            /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
            /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</returns>
            [__DynamicallyInvokable]
            public Dictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
            }
            /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="array" /> is null. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///   <paramref name="index" /> is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
            [__DynamicallyInvokable]
            public void CopyTo(TKey[] array, int index)
            {
                if (array == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
                if (index < 0 || index > array.Length)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
                if (array.Length - index < this.dictionary.Count)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                int count = this.dictionary.count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].hashCode >= 0)
                    {
                        array[index++] = entries[i].key;
                    }
                }
            }
            [__DynamicallyInvokable]
            void ICollection<TKey>.Add(TKey item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
            }
            [__DynamicallyInvokable]
            void ICollection<TKey>.Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
            }
            [__DynamicallyInvokable]
            bool ICollection<TKey>.Contains(TKey item)
            {
                return this.dictionary.ContainsKey(item);
            }
            [__DynamicallyInvokable]
            bool ICollection<TKey>.Remove(TKey item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
                return false;
            }
            [__DynamicallyInvokable]
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
            }
            /// <summary>Returns an enumerator that iterates through a collection.</summary>
            /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
            [__DynamicallyInvokable]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
            }
            /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="array" /> is null.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///   <paramref name="index" /> is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">
            ///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
            [__DynamicallyInvokable]
            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
                if (array.Rank != 1)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                }
                if (array.GetLowerBound(0) != 0)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                }
                if (index < 0 || index > array.Length)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
                if (array.Length - index < this.dictionary.Count)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                TKey[] array2 = array as TKey[];
                if (array2 != null)
                {
                    this.CopyTo(array2, index);
                    return;
                }
                object[] array3 = array as object[];
                if (array3 == null)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
                int count = this.dictionary.count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (entries[i].hashCode >= 0)
                        {
                            array3[index++] = entries[i].key;
                        }
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }
        }
        /// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited. </summary>
        [__DynamicallyInvokable]
        [Serializable]
        public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {
            /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
            [__DynamicallyInvokable]
            [Serializable]
            public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
            {
                private Dictionary<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TValue currentValue;
                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> at the current position of the enumerator.</returns>
                [__DynamicallyInvokable]
                public TValue Current
                {
                    [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
                    get
                    {
                        return this.currentValue;
                    }
                }
                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the collection at the current position of the enumerator.</returns>
                /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
                [__DynamicallyInvokable]
                object IEnumerator.Current
                {
                    [__DynamicallyInvokable]
                    get
                    {
                        if (this.index == 0 || this.index == this.dictionary.count + 1)
                        {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                        }
                        return this.currentValue;
                    }
                }
                internal Enumerator(Dictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;
                    this.index = 0;
                    this.currentValue = default(TValue);
                }
                /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" />.</summary>
                [__DynamicallyInvokable]
                public void Dispose()
                {
                }
                /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
                /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                [__DynamicallyInvokable]
                public bool MoveNext()
                {
                    if (this.version != this.dictionary.version)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
                    while (this.index < this.dictionary.count)
                    {
                        if (this.dictionary.entries[this.index].hashCode >= 0)
                        {
                            this.currentValue = this.dictionary.entries[this.index].value;
                            this.index++;
                            return true;
                        }
                        this.index++;
                    }
                    this.index = this.dictionary.count + 1;
                    this.currentValue = default(TValue);
                    return false;
                }
                /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                [__DynamicallyInvokable]
                void IEnumerator.Reset()
                {
                    if (this.version != this.dictionary.version)
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
                    this.index = 0;
                    this.currentValue = default(TValue);
                }
            }
            private Dictionary<TKey, TValue> dictionary;
            /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
            [__DynamicallyInvokable]
            public int Count
            {
                [__DynamicallyInvokable]
                get
                {
                    return this.dictionary.Count;
                }
            }
            [__DynamicallyInvokable]
            bool ICollection<TValue>.IsReadOnly
            {
                [__DynamicallyInvokable]
                get
                {
                    return true;
                }
            }
            /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
            /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns false.</returns>
            [__DynamicallyInvokable]
            bool ICollection.IsSynchronized
            {
                [__DynamicallyInvokable]
                get
                {
                    return false;
                }
            }
            /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
            /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns the current instance.</returns>
            [__DynamicallyInvokable]
            object ICollection.SyncRoot
            {
                [__DynamicallyInvokable]
                get
                {
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }
            /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> class that reflects the values in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
            /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose values are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="dictionary" /> is null.</exception>
            [__DynamicallyInvokable]
            public ValueCollection(Dictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
                }
                this.dictionary = dictionary;
            }
            /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
            /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
            [__DynamicallyInvokable]
            public Dictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
            }
            /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="array" /> is null.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///   <paramref name="index" /> is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
            [__DynamicallyInvokable]
            public void CopyTo(TValue[] array, int index)
            {
                if (array == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
                if (index < 0 || index > array.Length)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
                if (array.Length - index < this.dictionary.Count)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                int count = this.dictionary.count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].hashCode >= 0)
                    {
                        array[index++] = entries[i].value;
                    }
                }
            }
            [__DynamicallyInvokable]
            void ICollection<TValue>.Add(TValue item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
            }
            [__DynamicallyInvokable]
            bool ICollection<TValue>.Remove(TValue item)
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
                return false;
            }
            [__DynamicallyInvokable]
            void ICollection<TValue>.Clear()
            {
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
            }
            [__DynamicallyInvokable]
            bool ICollection<TValue>.Contains(TValue item)
            {
                return this.dictionary.ContainsValue(item);
            }
            [__DynamicallyInvokable]
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
            }
            /// <summary>Returns an enumerator that iterates through a collection.</summary>
            /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
            [__DynamicallyInvokable]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
            }
            /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="array" /> is null.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///   <paramref name="index" /> is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">
            ///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
            [__DynamicallyInvokable]
            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
                if (array.Rank != 1)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                }
                if (array.GetLowerBound(0) != 0)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                }
                if (index < 0 || index > array.Length)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
                if (array.Length - index < this.dictionary.Count)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                TValue[] array2 = array as TValue[];
                if (array2 != null)
                {
                    this.CopyTo(array2, index);
                    return;
                }
                object[] array3 = array as object[];
                if (array3 == null)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
                int count = this.dictionary.count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (entries[i].hashCode >= 0)
                        {
                            array3[index++] = entries[i].value;
                        }
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }
        }
        private int[] buckets;
        private Dictionary<TKey, TValue>.Entry[] entries;
        private int count;
        private int version;
        private int freeList;
        private int freeCount;
        private IEqualityComparer<TKey> comparer;
        private Dictionary<TKey, TValue>.KeyCollection keys;
        private Dictionary<TKey, TValue>.ValueCollection values;
        private object _syncRoot;
        private const string VersionName = "Version";
        private const string HashSizeName = "HashSize";
        private const string KeyValuePairsName = "KeyValuePairs";
        private const string ComparerName = "Comparer";
        /// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> that is used to determine equality of keys for the dictionary. </summary>
        /// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2" /> and to provide hash values for the keys.</returns>
        [__DynamicallyInvokable]
        public IEqualityComparer<TKey> Comparer
        {
            [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.comparer;
            }
        }
        /// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        /// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        [__DynamicallyInvokable]
        public int Count
        {
            [__DynamicallyInvokable]
            get
            {
                return this.count - this.freeCount;
            }
        }
        /// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        [__DynamicallyInvokable]
        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.keys == null)
                {
                    this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
                }
                return this.keys;
            }
        }
        [__DynamicallyInvokable]
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.keys == null)
                {
                    this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
                }
                return this.keys;
            }
        }
        [__DynamicallyInvokable]
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.keys == null)
                {
                    this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
                }
                return this.keys;
            }
        }
        /// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        [__DynamicallyInvokable]
        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.values == null)
                {
                    this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
                }
                return this.values;
            }
        }
        [__DynamicallyInvokable]
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.values == null)
                {
                    this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
                }
                return this.values;
            }
        }
        [__DynamicallyInvokable]
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            [__DynamicallyInvokable]
            get
            {
                if (this.values == null)
                {
                    this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
                }
                return this.values;
            }
        }
        /// <summary>Gets or sets the value associated with the specified key.</summary>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
        /// <param name="key">The key of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
        [__DynamicallyInvokable]
        public TValue this[TKey key]
        {
            [__DynamicallyInvokable]
            get
            {
                int num = this.FindEntry(key);
                if (num >= 0)
                {
                    return this.entries[num].value;
                }
                ThrowHelper.ThrowKeyNotFoundException();
                return default(TValue);
            }
            [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this.Insert(key, value, false);
            }
        }
        [__DynamicallyInvokable]
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            [__DynamicallyInvokable]
            get
            {
                return false;
            }
        }
        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
        [__DynamicallyInvokable]
        bool ICollection.IsSynchronized
        {
            [__DynamicallyInvokable]
            get
            {
                return false;
            }
        }
        /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. </returns>
        [__DynamicallyInvokable]
        object ICollection.SyncRoot
        {
            [__DynamicallyInvokable]
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }
        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
        /// <returns>true if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
        [__DynamicallyInvokable]
        bool IDictionary.IsFixedSize
        {
            [__DynamicallyInvokable]
            get
            {
                return false;
            }
        }
        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
        [__DynamicallyInvokable]
        bool IDictionary.IsReadOnly
        {
            [__DynamicallyInvokable]
            get
            {
                return false;
            }
        }
        /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
        [__DynamicallyInvokable]
        ICollection IDictionary.Keys
        {
            [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.Keys;
            }
        }
        /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
        [__DynamicallyInvokable]
        ICollection IDictionary.Values
        {
            [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.Values;
            }
        }
        /// <summary>Gets or sets the value with the specified key.</summary>
        /// <returns>The value associated with the specified key, or null if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        /// <param name="key">The key of the value to get.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-A value is being assigned, and <paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
        [__DynamicallyInvokable]
        object IDictionary.this[object key]
        {
            [__DynamicallyInvokable]
            get
            {
                if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
                {
                    int num = this.FindEntry((TKey)((object)key));
                    if (num >= 0)
                    {
                        return this.entries[num].value;
                    }
                }
                return null;
            }
            [__DynamicallyInvokable]
            set
            {
                if (key == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
                }
                ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
                try
                {
                    TKey key2 = (TKey)((object)key);
                    try
                    {
                        this[key2] = (TValue)((object)value);
                    }
                    catch (InvalidCastException)
                    {
                        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
                    }
                }
                catch (InvalidCastException)
                {
                    ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
                }
            }
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Dictionary()
            : this(0, null)
        {
            this.Initialize(0);
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="capacity" /> is less than 0.</exception>
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Dictionary(int capacity)
            : this(capacity, null)
        {
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Dictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="capacity" /> is less than 0.</exception>
        [__DynamicallyInvokable]
        public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
            }
            if (capacity > 0)
            {
                this.Initialize(capacity);
            }
            this.comparer = (comparer ?? EqualityComparer<TKey>.Default);
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the default equality comparer for the key type.</summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="dictionary" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Dictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="dictionary" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
        [__DynamicallyInvokable]
        public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this((dictionary != null) ? dictionary.Count : 0, comparer)
        {
            if (dictionary == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
            }
            foreach (KeyValuePair<TKey, TValue> current in dictionary)
            {
                this.Add(current.Key, current.Value);
            }
        }
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class with serialized data.</summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected Dictionary(SerializationInfo info, StreamingContext context)
        {
            HashHelpers.SerializationInfoTable.Add(this, info);
        }
        /// <summary>Adds the specified key and value to the dictionary.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            int num = this.comparer.GetHashCode(key) & 2147483647;
            int num2 = num % this.buckets.Length;

            int num4;
            if (this.freeCount > 0)
            {
                num4 = this.freeList;
                this.freeList = this.entries[num4].next;
                this.freeCount--;
            }
            else
            {
                if (this.count == this.entries.Length)
                {
                    this.Resize();
                    num2 = num % this.buckets.Length;
                }
                num4 = this.count;
                this.count++;
            }
            this.entries[num4].hashCode = num;
            this.entries[num4].next = this.buckets[num2];
            this.entries[num4].key = key;
            this.entries[num4].value = value;
            this.buckets[num2] = num4;
            this.version++;
        }
        [__DynamicallyInvokable]
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }
        [__DynamicallyInvokable]
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int num = this.FindEntry(keyValuePair.Key);
            return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value);
        }
        [__DynamicallyInvokable]
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int num = this.FindEntry(keyValuePair.Key);
            if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value))
            {
                this.Remove(keyValuePair.Key);
                return true;
            }
            return false;
        }
        /// <summary>Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        [__DynamicallyInvokable]
        public void Clear()
        {
            if (this.count > 0)
            {
                for (int i = 0; i < this.buckets.Length; i++)
                {
                    this.buckets[i] = -1;
                }
                Array.Clear(this.entries, 0, this.count);
                this.freeList = -1;
                this.count = 0;
                this.freeCount = 0;
                this.version++;
            }
        }
        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        [__DynamicallyInvokable]
        public bool ContainsKey(TKey key)
        {
            return this.FindEntry(key) >= 0;
        }
        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains a specific value.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified value; otherwise, false.</returns>
        /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />. The value can be null for reference types.</param>
        [__DynamicallyInvokable]
        public bool ContainsValue(TValue value)
        {
            if (value == null)
            {
                for (int i = 0; i < this.count; i++)
                {
                    if (this.entries[i].hashCode >= 0 && this.entries[i].value == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
                for (int j = 0; j < this.count; j++)
                {
                    if (this.entries[j].hashCode >= 0 && @default.Equals(this.entries[j].value, value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            if (index < 0 || index > array.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
            if (array.Length - index < this.Count)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }
            int num = this.count;
            Dictionary<TKey, TValue>.Entry[] array2 = this.entries;
            for (int i = 0; i < num; i++)
            {
                if (array2[i].hashCode >= 0)
                {
                    array[index++] = new KeyValuePair<TKey, TValue>(array2[i].key, array2[i].value);
                }
            }
        }
        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        [__DynamicallyInvokable]
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return new Dictionary<TKey, TValue>.Enumerator(this, 2);
        }
        [__DynamicallyInvokable]
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Dictionary<TKey, TValue>.Enumerator(this, 2);
        }
        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="info" /> is null.</exception>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
            }
            info.AddValue("Version", this.version);
            info.AddValue("Comparer", HashHelpers.GetEqualityComparerForSerialization(this.comparer), typeof(IEqualityComparer<TKey>));
            info.AddValue("HashSize", (this.buckets == null) ? 0 : this.buckets.Length);
            if (this.buckets != null)
            {
                KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
                this.CopyTo(array, 0);
                info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindEntry(TKey key)
        {
            int num = this.comparer.GetHashCode(key) & 2147483647;
            for (int i = this.buckets[num % this.buckets.Length]; i >= 0; i = this.entries[i].next)
            {
                if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
                {
                    return i;
                }
            }

            return -1;
        }
        private void Initialize(int capacity)
        {
            int prime = HashHelpers.GetPrime(capacity);
            this.buckets = new int[prime];
            for (int i = 0; i < this.buckets.Length; i++)
            {
                this.buckets[i] = -1;
            }
            this.entries = new Dictionary<TKey, TValue>.Entry[prime];
            this.freeList = -1;
        }
        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }
            if (this.buckets == null)
            {
                this.Initialize(0);
            }
            int num = this.comparer.GetHashCode(key) & 2147483647;
            int num2 = num % this.buckets.Length;
            int num3 = 0;
            for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
            {
                if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
                {
                    if (add)
                    {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
                    }
                    this.entries[i].value = value;
                    this.version++;
                    return;
                }
                num3++;
            }
            int num4;
            if (this.freeCount > 0)
            {
                num4 = this.freeList;
                this.freeList = this.entries[num4].next;
                this.freeCount--;
            }
            else
            {
                if (this.count == this.entries.Length)
                {
                    this.Resize();
                    num2 = num % this.buckets.Length;
                }
                num4 = this.count;
                this.count++;
            }
            this.entries[num4].hashCode = num;
            this.entries[num4].next = this.buckets[num2];
            this.entries[num4].key = key;
            this.entries[num4].value = value;
            this.buckets[num2] = num4;
            this.version++;
            if (num3 > 100 && HashHelpers.IsWellKnownEqualityComparer(this.comparer))
            {
                this.comparer = (IEqualityComparer<TKey>)HashHelpers.GetRandomizedEqualityComparer(this.comparer);
                this.Resize(this.entries.Length, true);
            }
        }
        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2" /> instance is invalid.</exception>
        public virtual void OnDeserialization(object sender)
        {
            SerializationInfo serializationInfo;
            HashHelpers.SerializationInfoTable.TryGetValue(this, out serializationInfo);
            if (serializationInfo == null)
            {
                return;
            }
            int @int = serializationInfo.GetInt32("Version");
            int int2 = serializationInfo.GetInt32("HashSize");
            this.comparer = (IEqualityComparer<TKey>)serializationInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
            if (int2 != 0)
            {
                this.buckets = new int[int2];
                for (int i = 0; i < this.buckets.Length; i++)
                {
                    this.buckets[i] = -1;
                }
                this.entries = new Dictionary<TKey, TValue>.Entry[int2];
                this.freeList = -1;
                KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])serializationInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
                if (array == null)
                {
                    ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeys);
                }
                for (int j = 0; j < array.Length; j++)
                {
                    if (array[j].Key == null)
                    {
                        ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
                    }
                    this.Insert(array[j].Key, array[j].Value, true);
                }
            }
            else
            {
                this.buckets = null;
            }
            this.version = @int;
            HashHelpers.SerializationInfoTable.Remove(this);
        }
        private void Resize()
        {
            this.Resize(HashHelpers.ExpandPrime(this.count), false);
        }
        private void Resize(int newSize, bool forceNewHashCodes)
        {
            int[] array = new int[newSize];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = -1;
            }
            Dictionary<TKey, TValue>.Entry[] array2 = new Dictionary<TKey, TValue>.Entry[newSize];
            Array.Copy(this.entries, 0, array2, 0, this.count);
            if (forceNewHashCodes)
            {
                for (int j = 0; j < this.count; j++)
                {
                    if (array2[j].hashCode != -1)
                    {
                        array2[j].hashCode = (this.comparer.GetHashCode(array2[j].key) & 2147483647);
                    }
                }
            }
            for (int k = 0; k < this.count; k++)
            {
                if (array2[k].hashCode >= 0)
                {
                    int num = array2[k].hashCode % newSize;
                    array2[k].next = array[num];
                    array[num] = k;
                }
            }
            this.buckets = array;
            this.entries = array2;
        }
        /// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        [__DynamicallyInvokable]
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }
            if (this.buckets != null)
            {
                int num = this.comparer.GetHashCode(key) & 2147483647;
                int num2 = num % this.buckets.Length;
                int num3 = -1;
                for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
                {
                    if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
                    {
                        if (num3 < 0)
                        {
                            this.buckets[num2] = this.entries[i].next;
                        }
                        else
                        {
                            this.entries[num3].next = this.entries[i].next;
                        }
                        this.entries[i].hashCode = -1;
                        this.entries[i].next = this.freeList;
                        this.entries[i].key = default(TKey);
                        this.entries[i].value = default(TValue);
                        this.freeList = i;
                        this.freeCount++;
                        this.version++;
                        return true;
                    }
                    num3 = i;
                }
            }
            return false;
        }
        /// <summary>Gets the value associated with the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        [__DynamicallyInvokable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            int num = this.FindEntry(key);
            if (num >= 0)
            {
                value = this.entries[num].value;
                return true;
            }
            value = default(TValue);
            return false;
        }
        internal TValue GetValueOrDefault(TKey key)
        {
            int num = this.FindEntry(key);
            if (num >= 0)
            {
                return this.entries[num].value;
            }
            return default(TValue);
        }
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            this.CopyTo(array, index);
        }
        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index" /> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.Generic.ICollection`1" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
        [__DynamicallyInvokable]
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            if (array.Rank != 1)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }
            if (array.GetLowerBound(0) != 0)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }
            if (index < 0 || index > array.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
            if (array.Length - index < this.Count)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }
            KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
            if (array2 != null)
            {
                this.CopyTo(array2, index);
                return;
            }
            if (array is DictionaryEntry[])
            {
                DictionaryEntry[] array3 = array as DictionaryEntry[];
                Dictionary<TKey, TValue>.Entry[] array4 = this.entries;
                for (int i = 0; i < this.count; i++)
                {
                    if (array4[i].hashCode >= 0)
                    {
                        array3[index++] = new DictionaryEntry(array4[i].key, array4[i].value);
                    }
                }
                return;
            }
            object[] array5 = array as object[];
            if (array5 == null)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            }
            try
            {
                int num = this.count;
                Dictionary<TKey, TValue>.Entry[] array6 = this.entries;
                for (int j = 0; j < num; j++)
                {
                    if (array6[j].hashCode >= 0)
                    {
                        array5[index++] = new KeyValuePair<TKey, TValue>(array6[j].key, array6[j].value);
                    }
                }
            }
            catch (ArrayTypeMismatchException)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
            }
        }
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
        [__DynamicallyInvokable]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Dictionary<TKey, TValue>.Enumerator(this, 2);
        }
        private static bool IsCompatibleKey(object key)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }
            return key is TKey;
        }
        /// <summary>Adds the specified key and value to the dictionary.</summary>
        /// <param name="key">The object to use as the key.</param>
        /// <param name="value">The object to use as the value.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-<paramref name="value" /> is of a type that is not assignable to <paramref name="TValue" />, the type of values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-A value with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
        [__DynamicallyInvokable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IDictionary.Add(object key, object value)
        {
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
            }
            ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
            try
            {
                TKey key2 = (TKey)((object)key);
                try
                {
                    this.Add(key2, (TValue)((object)value));
                }
                catch (InvalidCastException)
                {
                    ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
                }
            }
            catch (InvalidCastException)
            {
                ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
            }
        }
        /// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        [__DynamicallyInvokable]
        bool IDictionary.Contains(object key)
        {
            return Dictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
        }
        /// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</summary>
        /// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
        [__DynamicallyInvokable]
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new Dictionary<TKey, TValue>.Enumerator(this, 1);
        }
        /// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key" /> is null.</exception>
        [__DynamicallyInvokable]
        void IDictionary.Remove(object key)
        {
            if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
            {
                this.Remove((TKey)((object)key));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = false, Inherited = false), FriendAccessAllowed]
    internal sealed class FriendAccessAllowedAttribute : Attribute
    {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public FriendAccessAllowedAttribute()
        {
        }
    }

    [FriendAccessAllowed]
    internal static class HashHelpers
    {
        public const int HashCollisionThreshold = 100;
        public const int MaxPrimeArrayLength = 2146435069;
        public static bool s_UseRandomizedStringHashing = false;
        public static readonly int[] primes = new int[]
		{
			3,
			7,
			11,
			17,
			23,
			29,
			37,
			47,
			59,
			71,
			89,
			107,
			131,
			163,
			197,
			239,
			293,
			353,
			431,
			521,
			631,
			761,
			919,
			1103,
			1327,
			1597,
			1931,
			2333,
			2801,
			3371,
			4049,
			4861,
			5839,
			7013,
			8419,
			10103,
			12143,
			14591,
			17519,
			21023,
			25229,
			30293,
			36353,
			43627,
			52361,
			62851,
			75431,
			90523,
			108631,
			130363,
			156437,
			187751,
			225307,
			270371,
			324449,
			389357,
			467237,
			560689,
			672827,
			807403,
			968897,
			1162687,
			1395263,
			1674319,
			2009191,
			2411033,
			2893249,
			3471899,
			4166287,
			4999559,
			5999471,
			7199369
		};
        private static ConditionalWeakTable<object, SerializationInfo> s_SerializationInfoTable;
        private static RandomNumberGenerator rng;
        private static byte[] data;
        private static int currentIndex = 1024;
        private static readonly object lockObj = new object();
        private const int bufferSize = 1024;
        internal static ConditionalWeakTable<object, SerializationInfo> SerializationInfoTable
        {
            get
            {
                if (HashHelpers.s_SerializationInfoTable == null)
                {
                    ConditionalWeakTable<object, SerializationInfo> value = new ConditionalWeakTable<object, SerializationInfo>();
                    Interlocked.CompareExchange<ConditionalWeakTable<object, SerializationInfo>>(ref HashHelpers.s_SerializationInfoTable, value, null);
                }
                return HashHelpers.s_SerializationInfoTable;
            }
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int num = (int)Math.Sqrt((double)candidate);
                for (int i = 3; i <= num; i += 2)
                {
                    if (candidate % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            return candidate == 2;
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int GetPrime(int min)
        {
            if (min < 0)
            {
                throw new ArgumentException();
            }
            for (int i = 0; i < HashHelpers.primes.Length; i++)
            {
                int num = HashHelpers.primes[i];
                if (num >= min)
                {
                    return num;
                }
            }
            for (int j = min | 1; j < 2147483647; j += 2)
            {
                if (HashHelpers.IsPrime(j) && (j - 1) % 101 != 0)
                {
                    return j;
                }
            }
            return min;
        }
        public static int GetMinPrime()
        {
            return HashHelpers.primes[0];
        }
        public static int ExpandPrime(int oldSize)
        {
            int num = 2 * oldSize;
            if (num > 2146435069 && 2146435069 > oldSize)
            {
                return 2146435069;
            }
            return HashHelpers.GetPrime(num);
        }
        public static bool IsWellKnownEqualityComparer(object comparer)
        {
            return comparer == null || comparer == EqualityComparer<string>.Default || comparer is IWellKnownStringEqualityComparer;
        }
        public static IEqualityComparer GetRandomizedEqualityComparer(object comparer)
        {
            if (comparer == null)
            {
                return new RandomizedObjectEqualityComparer();
            }
            if (comparer == EqualityComparer<string>.Default)
            {
                return new RandomizedStringEqualityComparer();
            }
            IWellKnownStringEqualityComparer wellKnownStringEqualityComparer = comparer as IWellKnownStringEqualityComparer;
            if (wellKnownStringEqualityComparer != null)
            {
                return wellKnownStringEqualityComparer.GetRandomizedEqualityComparer();
            }
            return null;
        }
        public static object GetEqualityComparerForSerialization(object comparer)
        {
            if (comparer == null)
            {
                return null;
            }
            IWellKnownStringEqualityComparer wellKnownStringEqualityComparer = comparer as IWellKnownStringEqualityComparer;
            if (wellKnownStringEqualityComparer != null)
            {
                return wellKnownStringEqualityComparer.GetEqualityComparerForSerialization();
            }
            return comparer;
        }
        internal static long GetEntropy()
        {
            long result;
            lock (HashHelpers.lockObj)
            {
                if (HashHelpers.currentIndex == 1024)
                {
                    if (HashHelpers.rng == null)
                    {
                        HashHelpers.rng = RandomNumberGenerator.Create();
                        HashHelpers.data = new byte[1024];
                    }
                    HashHelpers.rng.GetBytes(HashHelpers.data);
                    HashHelpers.currentIndex = 0;
                }
                long num = BitConverter.ToInt64(HashHelpers.data, HashHelpers.currentIndex);
                HashHelpers.currentIndex += 8;
                result = num;
            }
            return result;
        }
    }
    
    internal interface IWellKnownStringEqualityComparer
    {
        IEqualityComparer GetRandomizedEqualityComparer();
        IEqualityComparer GetEqualityComparerForSerialization();
    }

    internal sealed class RandomizedObjectEqualityComparer : IEqualityComparer, IWellKnownStringEqualityComparer
    {
        private long _entropy;
        public RandomizedObjectEqualityComparer()
        {
            this._entropy = HashHelpers.GetEntropy();
        }
        public new bool Equals(object x, object y)
        {
            if (x != null)
            {
                return y != null && x.Equals(y);
            }
            return y == null;
        }
        [SecuritySafeCritical]
        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            RandomizedObjectEqualityComparer randomizedObjectEqualityComparer = obj as RandomizedObjectEqualityComparer;
            return randomizedObjectEqualityComparer != null && this._entropy == randomizedObjectEqualityComparer._entropy;
        }
        public override int GetHashCode()
        {
            return base.GetType().Name.GetHashCode() ^ (int)(this._entropy & 2147483647L);
        }
        IEqualityComparer IWellKnownStringEqualityComparer.GetRandomizedEqualityComparer()
        {
            return new RandomizedObjectEqualityComparer();
        }
        IEqualityComparer IWellKnownStringEqualityComparer.GetEqualityComparerForSerialization()
        {
            return null;
        }
    }

    internal sealed class RandomizedStringEqualityComparer : IEqualityComparer<string>, IEqualityComparer, IWellKnownStringEqualityComparer
    {
        private long _entropy;
        public RandomizedStringEqualityComparer()
        {
            this._entropy = HashHelpers.GetEntropy();
        }
        public new bool Equals(object x, object y)
        {
            if (x == y)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            if (x is string && y is string)
            {
                return this.Equals((string)x, (string)y);
            }
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
            return false;
        }
        public bool Equals(string x, string y)
        {
            if (x != null)
            {
                return y != null && x.Equals(y);
            }
            return y == null;
        }
        [SecuritySafeCritical]
        public int GetHashCode(string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.GetHashCode();
        }
        [SecuritySafeCritical]
        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            RandomizedStringEqualityComparer randomizedStringEqualityComparer = obj as RandomizedStringEqualityComparer;
            return randomizedStringEqualityComparer != null && this._entropy == randomizedStringEqualityComparer._entropy;
        }
        public override int GetHashCode()
        {
            return base.GetType().Name.GetHashCode() ^ (int)(this._entropy & 2147483647L);
        }
        IEqualityComparer IWellKnownStringEqualityComparer.GetRandomizedEqualityComparer()
        {
            return new RandomizedStringEqualityComparer();
        }
        IEqualityComparer IWellKnownStringEqualityComparer.GetEqualityComparerForSerialization()
        {
            return EqualityComparer<string>.Default;
        }
    }

    internal enum ExceptionResource
    {
        Argument_ImplementIComparable,
        Argument_InvalidType,
        Argument_InvalidArgumentForComparison,
        Argument_InvalidRegistryKeyPermissionCheck,
        ArgumentOutOfRange_NeedNonNegNum,
        Arg_ArrayPlusOffTooSmall,
        Arg_NonZeroLowerBound,
        Arg_RankMultiDimNotSupported,
        Arg_RegKeyDelHive,
        Arg_RegKeyStrLenBug,
        Arg_RegSetStrArrNull,
        Arg_RegSetMismatchedKind,
        Arg_RegSubKeyAbsent,
        Arg_RegSubKeyValueAbsent,
        Argument_AddingDuplicate,
        Serialization_InvalidOnDeser,
        Serialization_MissingKeys,
        Serialization_NullKey,
        Argument_InvalidArrayType,
        NotSupported_KeyCollectionSet,
        NotSupported_ValueCollectionSet,
        ArgumentOutOfRange_SmallCapacity,
        ArgumentOutOfRange_Index,
        Argument_InvalidOffLen,
        Argument_ItemNotExist,
        ArgumentOutOfRange_Count,
        ArgumentOutOfRange_InvalidThreshold,
        ArgumentOutOfRange_ListInsert,
        NotSupported_ReadOnlyCollection,
        InvalidOperation_CannotRemoveFromStackOrQueue,
        InvalidOperation_EmptyQueue,
        InvalidOperation_EnumOpCantHappen,
        InvalidOperation_EnumFailedVersion,
        InvalidOperation_EmptyStack,
        ArgumentOutOfRange_BiggerThanCollection,
        InvalidOperation_EnumNotStarted,
        InvalidOperation_EnumEnded,
        NotSupported_SortedListNestedWrite,
        InvalidOperation_NoValue,
        InvalidOperation_RegRemoveSubKey,
        Security_RegistryPermission,
        UnauthorizedAccess_RegistryNoWrite,
        ObjectDisposed_RegKeyClosed,
        NotSupported_InComparableType,
        Argument_InvalidRegistryOptionsCheck,
        Argument_InvalidRegistryViewCheck
    }

    internal enum ExceptionArgument
    {
        obj,
        dictionary,
        dictionaryCreationThreshold,
        array,
        info,
        key,
        collection,
        list,
        match,
        converter,
        queue,
        stack,
        capacity,
        index,
        startIndex,
        value,
        count,
        arrayIndex,
        name,
        mode,
        item,
        options,
        view
    }

    internal static class ThrowHelper
    {
        internal static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(ExceptionArgument.index));
        }
        internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
        {
            throw new ArgumentException("key");
        }
        internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
        {
            throw new ArgumentException("value");
        }
        internal static void ThrowKeyNotFoundException()
        {
            throw new KeyNotFoundException();
        }
        internal static void ThrowArgumentException(ExceptionResource resource)
        {
            throw new ArgumentException();
        }
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
        {
            throw new ArgumentException();
        }
        internal static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
        }
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
        }
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
        {
            throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
        }
        internal static void ThrowInvalidOperationException(ExceptionResource resource)
        {
            throw new InvalidOperationException();
        }
        internal static void ThrowSerializationException(ExceptionResource resource)
        {
            throw new SerializationException();
        }
        internal static void ThrowSecurityException(ExceptionResource resource)
        {
            throw new SecurityException();
        }
        internal static void ThrowNotSupportedException(ExceptionResource resource)
        {
            throw new NotSupportedException();
        }
        internal static void ThrowUnauthorizedAccessException(ExceptionResource resource)
        {
            throw new UnauthorizedAccessException();
        }
        internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource)
        {
            throw new ObjectDisposedException(objectName);
        }
        internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, ExceptionArgument argName)
        {
            if (value == null && default(T) != null)
            {
                ThrowHelper.ThrowArgumentNullException(argName);
            }
        }
        internal static string GetArgumentName(ExceptionArgument argument)
        {
            string result;
            switch (argument)
            {
                case ExceptionArgument.obj:
                    result = "obj";
                    break;
                case ExceptionArgument.dictionary:
                    result = "dictionary";
                    break;
                case ExceptionArgument.dictionaryCreationThreshold:
                    result = "dictionaryCreationThreshold";
                    break;
                case ExceptionArgument.array:
                    result = "array";
                    break;
                case ExceptionArgument.info:
                    result = "info";
                    break;
                case ExceptionArgument.key:
                    result = "key";
                    break;
                case ExceptionArgument.collection:
                    result = "collection";
                    break;
                case ExceptionArgument.list:
                    result = "list";
                    break;
                case ExceptionArgument.match:
                    result = "match";
                    break;
                case ExceptionArgument.converter:
                    result = "converter";
                    break;
                case ExceptionArgument.queue:
                    result = "queue";
                    break;
                case ExceptionArgument.stack:
                    result = "stack";
                    break;
                case ExceptionArgument.capacity:
                    result = "capacity";
                    break;
                case ExceptionArgument.index:
                    result = "index";
                    break;
                case ExceptionArgument.startIndex:
                    result = "startIndex";
                    break;
                case ExceptionArgument.value:
                    result = "value";
                    break;
                case ExceptionArgument.count:
                    result = "count";
                    break;
                case ExceptionArgument.arrayIndex:
                    result = "arrayIndex";
                    break;
                case ExceptionArgument.name:
                    result = "name";
                    break;
                case ExceptionArgument.mode:
                    result = "mode";
                    break;
                case ExceptionArgument.item:
                    result = "item";
                    break;
                case ExceptionArgument.options:
                    result = "options";
                    break;
                case ExceptionArgument.view:
                    result = "view";
                    break;
                default:
                    return string.Empty;
            }
            return result;
        }
        internal static string GetResourceName(ExceptionResource resource)
        {
            string result;
            switch (resource)
            {
                case ExceptionResource.Argument_ImplementIComparable:
                    result = "Argument_ImplementIComparable";
                    break;
                case ExceptionResource.Argument_InvalidType:
                    result = "Argument_InvalidType";
                    break;
                case ExceptionResource.Argument_InvalidArgumentForComparison:
                    result = "Argument_InvalidArgumentForComparison";
                    break;
                case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
                    result = "Argument_InvalidRegistryKeyPermissionCheck";
                    break;
                case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
                    result = "ArgumentOutOfRange_NeedNonNegNum";
                    break;
                case ExceptionResource.Arg_ArrayPlusOffTooSmall:
                    result = "Arg_ArrayPlusOffTooSmall";
                    break;
                case ExceptionResource.Arg_NonZeroLowerBound:
                    result = "Arg_NonZeroLowerBound";
                    break;
                case ExceptionResource.Arg_RankMultiDimNotSupported:
                    result = "Arg_RankMultiDimNotSupported";
                    break;
                case ExceptionResource.Arg_RegKeyDelHive:
                    result = "Arg_RegKeyDelHive";
                    break;
                case ExceptionResource.Arg_RegKeyStrLenBug:
                    result = "Arg_RegKeyStrLenBug";
                    break;
                case ExceptionResource.Arg_RegSetStrArrNull:
                    result = "Arg_RegSetStrArrNull";
                    break;
                case ExceptionResource.Arg_RegSetMismatchedKind:
                    result = "Arg_RegSetMismatchedKind";
                    break;
                case ExceptionResource.Arg_RegSubKeyAbsent:
                    result = "Arg_RegSubKeyAbsent";
                    break;
                case ExceptionResource.Arg_RegSubKeyValueAbsent:
                    result = "Arg_RegSubKeyValueAbsent";
                    break;
                case ExceptionResource.Argument_AddingDuplicate:
                    result = "Argument_AddingDuplicate";
                    break;
                case ExceptionResource.Serialization_InvalidOnDeser:
                    result = "Serialization_InvalidOnDeser";
                    break;
                case ExceptionResource.Serialization_MissingKeys:
                    result = "Serialization_MissingKeys";
                    break;
                case ExceptionResource.Serialization_NullKey:
                    result = "Serialization_NullKey";
                    break;
                case ExceptionResource.Argument_InvalidArrayType:
                    result = "Argument_InvalidArrayType";
                    break;
                case ExceptionResource.NotSupported_KeyCollectionSet:
                    result = "NotSupported_KeyCollectionSet";
                    break;
                case ExceptionResource.NotSupported_ValueCollectionSet:
                    result = "NotSupported_ValueCollectionSet";
                    break;
                case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
                    result = "ArgumentOutOfRange_SmallCapacity";
                    break;
                case ExceptionResource.ArgumentOutOfRange_Index:
                    result = "ArgumentOutOfRange_Index";
                    break;
                case ExceptionResource.Argument_InvalidOffLen:
                    result = "Argument_InvalidOffLen";
                    break;
                case ExceptionResource.Argument_ItemNotExist:
                    result = "Argument_ItemNotExist";
                    break;
                case ExceptionResource.ArgumentOutOfRange_Count:
                    result = "ArgumentOutOfRange_Count";
                    break;
                case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
                    result = "ArgumentOutOfRange_InvalidThreshold";
                    break;
                case ExceptionResource.ArgumentOutOfRange_ListInsert:
                    result = "ArgumentOutOfRange_ListInsert";
                    break;
                case ExceptionResource.NotSupported_ReadOnlyCollection:
                    result = "NotSupported_ReadOnlyCollection";
                    break;
                case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
                    result = "InvalidOperation_CannotRemoveFromStackOrQueue";
                    break;
                case ExceptionResource.InvalidOperation_EmptyQueue:
                    result = "InvalidOperation_EmptyQueue";
                    break;
                case ExceptionResource.InvalidOperation_EnumOpCantHappen:
                    result = "InvalidOperation_EnumOpCantHappen";
                    break;
                case ExceptionResource.InvalidOperation_EnumFailedVersion:
                    result = "InvalidOperation_EnumFailedVersion";
                    break;
                case ExceptionResource.InvalidOperation_EmptyStack:
                    result = "InvalidOperation_EmptyStack";
                    break;
                case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
                    result = "ArgumentOutOfRange_BiggerThanCollection";
                    break;
                case ExceptionResource.InvalidOperation_EnumNotStarted:
                    result = "InvalidOperation_EnumNotStarted";
                    break;
                case ExceptionResource.InvalidOperation_EnumEnded:
                    result = "InvalidOperation_EnumEnded";
                    break;
                case ExceptionResource.NotSupported_SortedListNestedWrite:
                    result = "NotSupported_SortedListNestedWrite";
                    break;
                case ExceptionResource.InvalidOperation_NoValue:
                    result = "InvalidOperation_NoValue";
                    break;
                case ExceptionResource.InvalidOperation_RegRemoveSubKey:
                    result = "InvalidOperation_RegRemoveSubKey";
                    break;
                case ExceptionResource.Security_RegistryPermission:
                    result = "Security_RegistryPermission";
                    break;
                case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
                    result = "UnauthorizedAccess_RegistryNoWrite";
                    break;
                case ExceptionResource.ObjectDisposed_RegKeyClosed:
                    result = "ObjectDisposed_RegKeyClosed";
                    break;
                case ExceptionResource.NotSupported_InComparableType:
                    result = "NotSupported_InComparableType";
                    break;
                case ExceptionResource.Argument_InvalidRegistryOptionsCheck:
                    result = "Argument_InvalidRegistryOptionsCheck";
                    break;
                case ExceptionResource.Argument_InvalidRegistryViewCheck:
                    result = "Argument_InvalidRegistryViewCheck";
                    break;
                default:
                    return string.Empty;
            }
            return result;
        }
    }
}
