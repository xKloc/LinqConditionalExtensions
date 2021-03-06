[![](https://img.shields.io/nuget/v/LinqConditionalExtensions.svg)](https://www.nuget.org/packages/LinqConditionalExtensions) [![](https://img.shields.io/nuget/vpre/LinqConditionalExtensions.svg)](https://www.nuget.org/packages/LinqConditionalExtensions)

# LinqConditionalExtensions
These extensions make it easy to chain Linq expressions based on conditions—useful for sorting, filtering, and paging.

## Installation
### Package Manager
`Install-Package LinqConditionalExtensions`

### .NET CLI
`dotnet add package LinqConditionalExtensions`

## Conditionals
Any extension for `IEnumerable<T>` and `IQueryable<T>` that returns itself can be called with `If` appended to it and be conditionally applied.

Here is a sample for applying where clauses conditionally based on filters, ordering by a column, and paging the results. When using something like Entity Framework, this entire chain will be dynamically converted straight to SQL and make your query time much shorter.
```csharp
var results = _context.Employees
	.WhereIf(hasNameFilter, e => e.Name.Contains(nameFilter))
	.WhereIf(hasPositionFilter, e => e.Position.Contains(positionFilter))
	.WhereIf(hasIdFilter, e => e.Id == idFilter)
	.OrderByIf(columnSort == "Name", e => e.Name)
	.OrderByIf(columnSort == "Position", e => e.Position)
	.OrderByIf(columnSort == "Id", e => e.Id)
	.SkipIf(isPaged, pageNumber * resultsPerPage)
	.TakeIf(isPaged, resultsPerPage)
	.ToList();
```

## If
You can do anything based on a condition with a single if statement. In this example, if the condition is true, a where clause and order by clause is being applied to the list of employees.

```csharp
var query = employeeDirectory
    .If(hasNameFilter, employees => employees
        .Where(e => e.Name.Contains(nameFilter))
        .OrderBy(e => e.Department));
```

## If Chain
You can use an if-chain to add if statement logic to your queryable or enumerable. If chains require you to have an `Else()` call to end the statement. You can add as many `ElseIf()` conditions in between.

In this sample, a position and name is being used to filter a list of employees to find only the ones who fall under them on the org chart. The CEO returns all employees. The vice president and manager are used to filter by their respective properties. Otherwise, no employees are returned by applying a where false.

```csharp
var subordinates = employeeDirectory
	.IfChain(position == "CEO", employees => employees) \\ All employees are under the CEO
	.ElseIf(position == "VP", employees => employees
		.Where(employee => employee.VicePresidentName == name))	\\ Employees that have a vice president with the passed name
	.ElseIf(position == "Manager", employees => employees
		.Where(employee => employee.ManagerName == name)) \\ Employees that have a manager with the passed name
	.Else(employees => employees
		.Where(employee => false)); \\ No employees
```

In this sample, instead of returning the subordinates, we are just getting the count. You can do a transformation on the result. You just have to make sure each method in the chain returns the same type.

```csharp
var subordinateCount = employeeDirectory
	.IfChain(position == "CEO", employees => employees.Count())
	.ElseIf(position == "VP", employees => employees.Where(employee => employee.VicePresidentName == name).Count())
	.ElseIf(position == "Manager", employees => employees.Where(employee => employee.ManagerName == name).Count())
	.Else(employees => 0);
```

## Switch
You can use a switch statement to chain together case statements. This is really useful for applying sorts.

In this example, a switch chain is being used to order results based on the column sort string.

```csharp
var sortedResults = results
	.Switch(columnSort)
	.Case("Name", set => set.OrderBy(e => e.Name))
	.Case("Position", set => set.OrderBy(e => e.Position))
	.Case("VicePresidentName", set => set.OrderBy(e => e.VicePresidentName))
	.Case("ManagerName", set => set.OrderBy(e => e.ManagerName))
	.Default();
```

Since ordering is a common task, you can use the shorthand version with order by case. There is also a where case for quickly applying filters in a switch.

```csharp
var sortedResults = results
    .Switch(columnSort)
    .OrderByCase("Name", e => e.Name)
    .OrderByCase("Position", e => e.Position)
    .OrderByCase("VicePresidentName", e => e.VicePresidentName)
    .OrderByCase("ManagerName", e => e.ManagerName)
    .Default();
```

Also, there are overloads to use lamba expressions for each case if you need to do something more complex. For example, you can use a class or tuple to include the direction of a sort.

**Note:** Only the first true condition will be applied. The rest of the conditions will be ignored. If there is overlap, make sure you order your conditions appropriately.

```csharp
var sort = (column: "Name", direction: "asc");

var sortedResults3 = employeeDirectory
    .Switch(sort)
    .OrderByCase(e => e.column == "Name" && e.direction == "asc", e => e.Name)
    .OrderByDescendingCase(e => e.column == "Name" && e.direction == "desc", e => e.Name)
    .OrderByCase(e => e.column == "Position" && e.direction == "asc", e => e.Position)
    .OrderByDescendingCase(e => e.column == "Position" && e.direction == "desc", e => e.Position)
    .OrderByCase(e => e.column == "VicePresidentName" && e.direction == "asc", e => e.VicePresidentName)
    .OrderByDescendingCase(e => e.column == "VicePresidentName" && e.direction == "desc", e => e.VicePresidentName)
    .OrderByCase(e => e.column == "ManagerName" && e.direction == "asc", e => e.VicePresidentName)
    .OrderByDescendingCase(e => e.column == "ManagerName" && e.direction == "desc", e => e.VicePresidentName)
    .OrderByDefault(e => e.Name);
```

You can also do a transformation in the switch chain, but you'll have to specify the type.

In this example, the types are string for the switch, Employee for the source, and int for the result.

```csharp
var total = employeeDirectory
	.Switch<string, Employee, int>(department)
	.Case("IT", set => set.Where(e => e.Department == "Information Technology").Count())
	.Default(set => 0);
```
