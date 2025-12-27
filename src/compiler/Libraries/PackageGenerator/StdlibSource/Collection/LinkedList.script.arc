link Arc::Std::Compilation;
link Arc::Std::Console;
link Arc::Std::Array;

namespace Arc::Std::Collection
{
    public group LinkedList<T>
    {
        private field var head: LinkedListNode<T>;
        private field var tail: LinkedListNode<T>;
        private field var size: int;

        public constructor(var self: LinkedList<T>): LinkedList<T>
        {
            self.size = 0;
            self.head = none;
            self.tail = none;

            return self;
        }

        public func append(var self: LinkedList<T>, const v: T): none
        {
            var node: LinkedListNode<T> = new LinkedListNode<T>(v);

            if(self.size == 0)
            {
                self.head = node;
                self.tail = node;
            }
            else
            {
                self.tail.next = node;
                node.prev = self.tail;
                self.tail = node;
            }

            self.size = self.size + 1;
            return;
        }

        public func prepend(var self: LinkedList<T>, const v: T): none
        {
            var node: LinkedListNode<T> = new LinkedListNode<T>(v);

            if(self.size == 0)
            {
                self.head = node;
                self.tail = node;
            }
            else
            {
                self.head.prev = node;
                node.next = self.head;
                self.head = node;
            }

            self.size = self.size + 1;
            return;
        }

        public func getSize(var self: LinkedList<T>): int
        {
            return self.size;
        }

        public func at(var self: LinkedList<T>, const index: int): T
        {
            return self.nodeAt(index).value;
        }

        private func nodeAt(var self: LinkedList<T>, const index: int): LinkedListNode<T>
        {
            if(index < 0 || index >= self.size)
            {
                return none;
            }

            if(index == 0)
            {
                return self.head;
            }
            elif(index == self.size - 1)
            {
                return self.tail;
            }

            # Search from the beginning
            var curr: LinkedListNode<T> = self.head;
            for(var i: int = 0; i < index; i = i + 1)
            {
                curr = curr.next;
            }

            return curr;
        }

        private func insertAfter(var self: LinkedList<T>, const index: int, const v: T): none
        {
            if(index < 0 || index >= self.size)
            {
                return none;
            }

            if(index == 0)
            {
                call self.prepend(v);
                return;
            }
            elif(index == self.size - 1)
            {
                call self.append(v);
                return;
            }

            var node: LinkedListNode<T> = self.nodeAt(index);
            var target: LinkedListNode<T> = new LinkedListNode<T>(v);

            target.next = node.next;
            node.next = target;
            target.prev = node;

            return;
        }
    }

    public group LinkedListNode<T>
    {
        public field var value: T;
        public field var next: LinkedListNode<T>;
        public field var prev: LinkedListNode<T>;

        public constructor(var self: LinkedListNode<T>, const value: T): LinkedListNode<T>
        {
            self.value = value;
            return self;
        }

        public func clearNext(var self: LinkedListNode<T>): none
        {
            self.next = none;
        }

        public func clearPrev(var self: LinkedListNode<T>): none
        {
            self.prev = none;
        }
    }

    @Entrypoint
    public func main(const args: string[]): int {
        var ll: LinkedList<int> = new LinkedList<int>();

        call ll.append(1);
        call ll.append(2);
        call ll.append(3);

        call PrintInteger(ll.getSize());

        call PrintInteger(ll.at(1));

        return 0;
    }
}
