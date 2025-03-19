

namespace MiProyectoBackend.model.list
{
    public interface IList<E> : IEnumerable<E> {

        public int Size();
        public int Add(E element);
        public void Insert(int pos, E element);
        public E RemoveAt(int pos);
        public void Remove(E element);
        public E GetFirst();
        public E GetAt(int index);
        public int IndexOf(E element);

    }

}