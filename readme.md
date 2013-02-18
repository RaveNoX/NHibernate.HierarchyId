# NHibernate.HierarchyId

Adds support for MS SQL [HierarchyId](http://msdn.microsoft.com/library/bb677290.aspx) type to NHibernate 3

## Implementation status

* [GetAncestor](http://msdn.microsoft.com/library/bb677202.aspx) - (HQL / Linq / QueryOver)
* [GetDescendant](http://msdn.microsoft.com/library/bb677209.aspx) - (HQL /Linq / QueryOver)
* [GetLevel](http://msdn.microsoft.com/library/bb677197.aspx) - (HQL / Linq / QueryOver)
* [GetRoot](http://msdn.microsoft.com/library/bb677194.aspx) - not implemented
* [IsDescendantOf](http://msdn.microsoft.com/library/bb677203.aspx) - (HQL / Linq / QueryOver)
* [Parse](http://msdn.microsoft.com/library/bb677201.aspx) - (HQL / Linq / QueryOver)
* [Read](http://msdn.microsoft.com/library/bb677205.aspx) - not implemented
* [GetReparentedValue](http://msdn.microsoft.com/library/bb677207.aspx) - (HQL / Linq / QueryOver)
* [ToString](http://msdn.microsoft.com/library/bb677195.aspx) - (HQL / Linq / QueryOver)
* [Write](http://msdn.microsoft.com/library/bb677196.aspx) - not implemented


## Usage

For use all of this methods you must first register extensions in NH config:

	using NHibernate.HierarchyId;
	...
	HierarchyIdExtensions.RegisterTypes(NHibernateConfigInstance)


_For methods usage please see [Samples](master/Samples) folder_