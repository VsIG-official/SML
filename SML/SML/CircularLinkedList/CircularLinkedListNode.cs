namespace SML.CircularLinkedList;

public class CircularLinkedListNode<T>
{
    public T Data;
    public CircularLinkedListNode<T>? Next;

    public CircularLinkedListNode(T data)
    {
        Data = data;
        Next = null;
    }
}
