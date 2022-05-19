using System.Collections;
using System.Text;

namespace SML.CircularLinkedList;

public class CircularLinkedList<T> : ICollection<T>, IEnumerable<T>, ICloneable
{
    #region Fields

    public CircularLinkedListNode<T>? Head { get; private set; }
    public CircularLinkedListNode<T>? Tail { get; private set; }

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public event Action Added;
    public event Action Removed;

    #endregion Fields

    #region Constructors

    public CircularLinkedList()
    {
        Head = null;
        Tail = null;
    }

    public CircularLinkedList(T item)
    {
        SetFirstElement(item);
    }

    #endregion Constructors

    #region Methods

    public T this[int index]
    {
        get
        {
            CheckCorrectIndex(index);

            var current = GetNodeInRange(Head, index);

            return current.Data;
        }
        set
        {
            CheckNull(value);

            CheckCorrectIndex(index);

            var current = GetNodeInRange(Head, index);

            current.Data = value;
        }
    }

    public void Add(T item)
    {
        CheckNull(item);

        if (IsEmpty())
        {
            SetFirstElement(item);
            return;
        }

        Tail = new CircularLinkedListNode<T>(item);
        Tail.Next = Head;

        var current = GetNodeInRange(Head, Count - 1);

        current.Next = Tail;

        Count++;
        Added?.Invoke();
    }

    public void AddFirst(T item)
    {
        CheckNull(item);

        if (IsEmpty())
        {
            SetFirstElement(item);
            return;
        }

        Head = new CircularLinkedListNode<T>(item)
        {
            Next = Head
        };

        Count++;
        SetTail();
        Added?.Invoke();
    }

    public void AddAt(T item, int index)
    {
        CheckNull(item);
        CheckCorrectIndex(index);

        var current = GetNodeInRange(Head, index - 1);

        var next = current.Next;

        var nodeToInsert = new CircularLinkedListNode<T>(item);

        current.Next = nodeToInsert;
        nodeToInsert.Next = next;

        Count++;
        Added?.Invoke();
    }

    public void Clear()
    {
        Head = Tail = null;
        Count = 0;
        Removed?.Invoke();
    }

    public bool Contains(T item)
    {
        CheckNull(item);

        var current = Head;

        for (var i = 0; i < Count; i++)
        {
            if (Compare(current.Data, item))
            {
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    // regular "==" will only works when T is constrained to be a reference type
    // Without any constraints, you can compare with null, but only null - and
    // that comparison will always be false for non-nullable value types.
    private static bool Compare<T>(T x, T y) => EqualityComparer<T>.Default.Equals(x, y);

    public void CopyTo(T[] array, int arrayIndex)
    {
        var node = Head;

        for (var i = arrayIndex; i < Count; i++)
        {
            array[arrayIndex + i] = node.Data;
            node = node.Next;
        }
    }

    public bool Remove(T item)
    {
        CheckNull(item);

        var current = Head;

        for (var i = 0; i < Count; i++)
        {
            if (Compare(current.Data, item))
            {
                return RemoveAt(i);
            }

            current = current.Next;
        }

        return false;
    }

    public void RemoveAll(T item)
    {
        CheckNull(item);

        for (int i = Count; i > 0; i--)
        {
            Remove(item);
        }
    }

    public bool RemoveAt(int index)
    {
        CheckCorrectIndex(index);

        var current = Head;
        var previous = Tail;

        for (var i = 0; i < index; i++)
        {
            previous = current;
            current = current.Next;
        }

        previous.Next = current.Next;

        ChangeEdgeNodes(previous, index);

        Count--;
        Removed?.Invoke();
        return true;
    }

    public void RemoveHead()
    {
        RemoveAt(0);
    }

    public void RemoveTail()
    {
        RemoveAt(Count - 1);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var node = Head;

        for (var i = 0; i < Count; i++)
        {
            yield return node.Data;
            node = node.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static CircularLinkedListNode<T> GetNodeInRange
        (CircularLinkedListNode<T> startingNode, int position)
    {
        for (int i = 0; i < position; i++)
        {
            startingNode = startingNode.Next;
        }

        return startingNode;
    }

    private bool CheckCorrectIndex(int index)
    {
        if (index <= Count - 1)
        {
            return true;
        }

        throw new ArgumentOutOfRangeException(nameof(index));
    }

    private void SetFirstElement(T item)
    {
        CheckNull(item);

        var node = new CircularLinkedListNode<T>(item);

        Head = node;
        Head.Next = node;
        Tail = node;
        Tail.Next = node;

        Count++;
        Added?.Invoke();
    }

    private bool IsEmpty()
    {
        if (Count == 0)
        {
            return true;
        }

        return false;
    }

    private static void CheckNull(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
    }

    private void SetTail()
    {
        var currentNode = Head;

        for (var i = 0; i < Count; i++)
        {
            if (i == Count - 1)
            {
                Tail = currentNode;
            }

            currentNode = currentNode.Next;
        }

        Tail.Next = Head;
    }

    private void ChangeEdgeNodes(CircularLinkedListNode<T> previous, int index)
    {
        if (index == 0)
        {
            Head = previous.Next;
        }
        else if (index == Count - 1)
        {
            Tail = previous;
        }
    }

    public override string ToString()
    {
        StringBuilder list = new();

        var current = Head;

        for (int i = 0; i < Count; i++)
        {
            list.Append(current.Data.ToString());
            list.Append(Environment.NewLine + Environment.NewLine);

            current = current.Next;
        }

        return list.ToString();
    }

    public object Clone()
    {
        CircularLinkedList<T> list = new();
        var current = Head;

        for (int i = 0; i < Count; i++)
        {
            list.Add(current.Data);
            current = current.Next;
        }

        return list;
    }

    #endregion Methods
}
