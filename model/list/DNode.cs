namespace MiProyectoBackend.model.list{

    public interface IDNode<E>{
        public E GetElement();
        public void SetElement(E element);
        public IDNode<E>? GetPrev();
        public IDNode<E>? GetNext();
        public void SetPrev(IDNode<E>? node);
        public void SetNext(IDNode<E>? node);

    }

    public class DNode<E>(E element, IDNode<E>? prev, IDNode<E>? next) : IDNode<E>
    {
        private E element = element;
        private IDNode<E>? prev = prev;
        private IDNode<E>? next = next;

        public E GetElement()
        {
            return element;
        }
        public IDNode<E>? GetPrev()
        {
            return prev;
        }
        public IDNode<E>? GetNext()
        {
            return next;
        }
        public void SetPrev(IDNode<E>? prev){
            this.prev = prev;
        }
        public void SetNext(IDNode<E>? next){
            this.next = next;
        }

        public void SetElement(E element)
        {
            this.element = element;
        }
    }

}
