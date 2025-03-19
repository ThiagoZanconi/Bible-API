namespace MiProyectoBackend.model.list{
    public class Test(){
    }


    public class ListNode(int val = 0, ListNode? next = null)
    {
        public int val = val;
        public ListNode? next = next;
    }

    public class Solution {
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
        int carry = (l1.val+l2.val > 9) ? 1 : 0;
        ListNode toReturn = new ListNode((l1.val+l2.val)%10,null);
        ListNode current = toReturn;
        
        while(l1.next!=null){
            l1=l1.next;
            if(l2.next!=null){
                l2=l2.next;
                current.next = new ListNode((l1.val+l2.val+carry)%10,null);
                carry = (l1.val+l2.val+carry > 9) ? 1 : 0;
            }
            else{
                current.next = new ListNode((l1.val+carry)%10,null);
                carry = (l1.val+carry > 9) ? 1 : 0;
            }
        }
        while(l2.next!=null){
                l2=l2.next;
                current.next = new ListNode((l2.val+carry)%10,null);
                carry = (l2.val+carry > 9) ? 1 : 0;
        }
        if(carry==1){
            current.next = new ListNode(1,null);
        }

        return toReturn;
    }
}
}