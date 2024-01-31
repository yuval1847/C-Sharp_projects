package Queue;
public class Queue<T>
{
    private Node<T> first;
    private Node<T> last;


    /* הפעולה בונה ומחזירה תור ריק **/
    public Queue()
    {
        this.first = null;
        this.last = null;
    }
    /* הפעולה מכניסה את הערך x לסוף התור הנוכחי **/
    public void Insert(T x)
    {
        Node<T> temp = new Node<T>(x);
        if (first == null)
            first = temp;
        else
            last.SetNext(temp);
        last = temp;
    }
    /* הפעולה מוציאה ומחזירה את הערך הנמצא  בראש התור הנוכחי **/
    public T Remove()
    {
        T x = first.GetInfo();
        first = first.GetNext();
        if (first == null)
            last = null;
        return x;
    }
    /* הפעולה מחזירה את הערך הנמצא  בראש התור הנוכחי **/
    public T Head()
    {
        return first.GetInfo();
    }
    /* הפעולה מחזירה אמת אם התור הנוכחי ריק או שקר אחרת **/
    public boolean IsEmpty()
    {
        return first == null;
    }
  /* הפעולה מחזירה מחרוזת המתארת את התור הנוכחי */        
    public String ToString()
    {
        String s = "[";
        Node<T> p = this.first;
        while (p != null)
        {
            s = s + p.GetInfo().ToString();
            if (p.GetNext() != null)
                s = s + ",";
            p = p.GetNext();
        }
        s = s + "]";    
        return s;
    }
}
