using System.Collections;

namespace MiProyectoBackend.model.list{
    public class DoubleLinkedList<E> : IList<E>
    {

        private IDNode<E>? header = null;
        private IDNode<E>? tail = null;
        private int size;

        public int Size()
        {
            return size;
        }

        public int Add(E element)
        {

            if(size==0){
                header = new DNode<E>(element,null,null);
                tail = header;
            }
            else{
                IDNode<E> node = new DNode<E>(element,tail,null);
                tail!.SetNext(node);
                tail = node;
            }
            size++;
            return size-1;
        }

        public void Insert(int pos, E element)
        {
            throw new NotImplementedException();
        }

        public E RemoveAt(int pos)
        {
            throw new NotImplementedException();
        }

        public void Remove(E element)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(E element)
        {
            throw new NotImplementedException();
        }

        public E GetAt(int index){
            if(index >= size){
                throw new IndexOutOfRangeException("Error: El indice indicado en el metodo Get() esta fuera del rango actual de la lista");
            }
            IDNode<E> toReturn = header!;
            for(int i=0;i<index;i++){
                toReturn = toReturn.GetNext()!;
            }
            return toReturn.GetElement();
        }

        public IEnumerator<E> GetEnumerator()
        {
            return new ListEnumerator<E>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public E GetFirst()
        {
            if(size==0){
                throw new EmptyListException("Error: Se invoco el metodo GetFirst() cuando la lista estaba vacia");
            }
            else{
                return header!.GetElement();
            }
        }

        public override string ToString(){
            string toReturn ="";

            foreach (var s in this){
                toReturn+="- "+s;
            }
            return toReturn;
        }
    }

    public class ListEnumerator<E>(DoubleLinkedList<E> list) : IEnumerator<E>
    {

        private readonly DoubleLinkedList<E> list = list;

        private int position = -1;
        public E Current
        {
            get
            {
                try
                {
                    return list.GetAt(position);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (Current == null)
                {
                    throw new InvalidOperationException("Current is null.");
                }
                return Current;
            }
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            position ++;
            return position < list.Size();
        }

        public void Reset()
        {
            position = -1;
        }
    }
}