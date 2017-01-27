# APPLIED DATA STRUCTURESPractical and robust .NET-styled implementations of advanced data structures in C#, in which data structure classes are wrapped in .NET style and can get handy usage in .NET codes. Moreover, structures here are **extendable for maintaining extra information for algorithm problems** (e.g. assignments on Online Judges and ACM);This repository is originally a playground for learning *Data Structure* class of Tsinghua Univerity, ([Class Info Here](http://dsa.cs.tsinghua.edu.cn/~deng/ds/index.htm)), and also for reading the book *Introduction to Algorithms*. So data structures implemented here are mainly from the class and the book, but also some from Wiki pages.## IMPLENTATION ROADMAP**The final goal of this project is to implement *Advanced* data structures listed in the [*List of Data Structures* in wiki](https://en.wikipedia.org/wiki/List_of_data_structures).**Basic dynamic data structures implemented by the .NET framework will not be considered.(like [*ArrayList*](https://referencesource.microsoft.com/#mscorlib/system/collections/arraylist.cs), [*List(Vector)*](https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs), [*Queue*](https://referencesource.microsoft.com/#System/compmod/system/collections/generic/queue.cs), [*Stack*](https://referencesource.microsoft.com/#System/compmod/system/collections/generic/stack.cs), [*Hash Table*](https://referencesource.microsoft.com/#mscorlib/system/collections/hashtable.cs), [*(Doubly-)Linked List*](https://referencesource.microsoft.com/#System/compmod/system/collections/generic/linkedlist.cs), [*Bit Array*](https://referencesource.microsoft.com/#mscorlib/system/collections/bitarray.cs), [*Dictionary(Associate Array)*](https://referencesource.microsoft.com/#mscorlib/system/collections/generic/dictionary.cs), [*Collection(Bag)*](https://referencesource.microsoft.com/#mscorlib/system/collections/generic/icollection.cs), [*Set*](https://referencesource.microsoft.com/#System/compmod/system/collections/generic/iset.cs))Simple data structure based on these structure will not be implemented here as well.(like [Double-ended Queue(deque)](https://en.wikipedia.org/wiki/Double-ended_queue), [Matrix](https://en.wikipedia.org/w/index.php?title=Matrix_(computer_science)&redirect=no))Algorithms implementations are not the goal of this project, but some of them will obviously be implemented when implementing data structures. And algorithm associated will implemented as much as possible.### Implemented:#### Binary Trees:- [AVL Tree](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/BST/AVLTree.cs)- [Binary Search Tree - 二叉搜索树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/BST/BinarySearchTree.cs)- [Binary Tree - 二叉树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Binary/BinaryTree.cs)- [Cartesian Tree - 笛卡尔树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/BST/CartesianTree.cs) (a.k.a Randomized Binary Search Tree)- [Complete (Binary) Tree - 完全(二叉)树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Binary/CompactTree.cs)- [Doubly-chained Tree - 双链树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Binary/DoublyChainedTreeNode.cs) (a.k.a Left-child right-sibling binary tree, filial-heir chain)- [Splay Tree - 伸展树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/BST/SplayTree.cs)- [Threaded Binary Tree - 线索二叉树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Binary/ThreadedBinaryTreeNode.cs)- [Treap - 树堆](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/BST/Treap.cs)#### Heaps:- [Binary Heap - 二叉堆](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Heap/BinaryHeap.cs)#### Multiway Trees:- [K-ary Tree - K叉树](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Multiway/KWayTreeNode.cs)- [van Emde Boas Tree](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Multiway/vanEmdeBoasTree.cs)### In Plan:- B Trees (AA tree, B*, B+, 2-3 tree, 2-3-4 tree, (a,b)-tree)- Binomial Heap - 二项堆- Disjoint Set - 并查集- Fenwick Tree - 树状数组- Interval Tree - 区间树- KD Tree- Leftist - 左式堆/左偏堆- Quadtree - 四叉树/四分树- Range Tree - 范围树- Red Black Tree - 红黑树- Segment Tree - 线段树- Size Balanced Tree - SBT- Skip List - 跳表- Trie - 键树- Unrolled Linked List - 块状链表### Considering- Bloom Filter- Octree - 八叉树- Fibnacci Heap - 斐波那契堆- Skew Heap - 斜堆- Stree (research paper [here](http://www.siam.org/meetings/alenex04/abstacts/rdementiev.pdf))### *Other Specific Applications Implemented:#### Data Structures:- [Dictionary based on BST](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/TreeEx.cs): Implement IDictionary and ICollection- [Splay List](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/RangeList.cs): List that supports range insert/remove/edit/reverse operations in O(lgn) time per element.#### Algorithms:- Tree Traverse (in/pre/post/level order) - Binary: [Naive Ver](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/TreeEx.cs), [Robust Ver](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/Tree/Binary/BinaryTreeEnumerator.cs) - Multiway: [Naive Ver](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/TreeEx.cs)- [Tree Classification](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/TreeEx.cs) (perfect/full/strict/complete)- [Heap Sort](https://github.com/cmpute/AppliedDataStructures/blob/master/DataStructures/ExampleUsages/SortingEX.cs)Turn to [Example Usages](https://github.com/cmpute/AppliedDataStructures/tree/master/DataStructures/ExampleUsages) for more usage of these data structures.## REFERENCE- [*Data Structure*](http://dsa.cs.tsinghua.edu.cn/~deng/ds/index.htm), Tsinghua Univerity, credit to [Junhui Deng](http://dsa.cs.tsinghua.edu.cn/~deng/index.htm)- *Introduction to Algorithms*, the 3rd Edition- [*Data Structure* Pages](https://en.wikipedia.org/wiki/Data_structure) in Wiki- [*Dictionary of Algorithms and Data Structures* (DADS)](http://www.nist.gov/dads/)- Other github repos (like ones of [aalhour](https://github.com/aalhour/C-Sharp-Algorithms), [riyadparvez](https://github.com/riyadparvez/data-structures-csharp), [vosen](https://github.com/vosen/kora), [BenFradet](https://github.com/BenFradet/Algorithms), [lkitching](https://github.com/lkitching/NDS), [luchunminglu](https://github.com/luchunminglu/Introduce_To_Algorithm3/tree/master/Introduce_To_Algorithm3/Introduce_To_Algorithm3/Common), [l1pton17](https://github.com/l1pton17/NetDataStructures), [TelerikAcademy](https://github.com/TelerikAcademy/Data-Structures-and-Algorithms))- [*Useful Data Structures*](http://suanfazu.com/t/na-xie-shao-wei-ren-zhi-dan-fei-chang-you-yong-de-suan-fa-he-shu-ju-jie-gou/385)