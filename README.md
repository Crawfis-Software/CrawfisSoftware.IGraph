# CrawfisSoftware.IGraph

`CrawfisSoftware.IGraph` is a small set of C# interfaces (and a helper edge struct) for representing directed graphs.

This repository primarily defines **contracts** that other libraries/projects can implement. In your solution you mentioned you have concrete implementations for:

- Grid graphs (main use case)
- Complete graphs (no edge storage)
- Adjacency-list graphs

The examples below show how to use those implementations *through the interfaces in this package*.

## Target frameworks

This library targets `.NET Standard 2.1`, so it can be referenced from `.NET 8` (and other modern .NET TFMs).

## Namespaces

All types are in the `CrawfisSoftware.Collections.Graph` namespace.

```csharp
using CrawfisSoftware.Collections.Graph;
```

## Core concepts

### Directed edges

All interfaces model **directed** edges from a `From` node to a `To` node.

- For an undirected graph, represent each undirected connection as two directed edges (A→B and B→A).

### Edge payloads

Edges carry a value/label of type `E` (e.g., a weight, cost, label, or metadata). If you don't need edge data, use a placeholder type like `bool`, `byte`, or a small enum.

### Two variants: label-based vs index-based

There are two parallel APIs:

1. **Label-based graphs** using `IGraph<N, E>` (nodes are identified by a label type `N`).
2. **Index-based graphs** using `IIndexedGraph<N, E>` (nodes are identified by integer indices).

Many graph implementations can support both: label-based for ergonomics, index-based for performance.

## Interfaces in this repo

### `IGraph<N, E>`

Use when your nodes are best identified by a label type (`N`) directly (e.g., `(int x, int y)`, `string`, an id type, etc.).

Main members:

- `IEnumerable<N> Nodes` — enumerate node labels
- `IEnumerable<N> Neighbors(N node)` — enumerate outgoing neighbor labels
- `IEnumerable<IEdge<N, E>> OutEdges(N node)` — enumerate outgoing edges
- `IEnumerable<IEdge<N, E>> Edges` — enumerate all edges
- `bool ContainsEdge(in N fromNode, in N toNode)`
- `E GetEdgeLabel(in N fromNode, in N toNode)`
- `bool TryGetEdge(in N fromNode, in N toNode, out E edge)`

Optional/inbound members (may not be supported by all implementations):

- `IEnumerable<N> Parents(N node)`
- `IEnumerable<IEdge<N, E>> InEdges(N node)`

### `IFiniteGraph<N, E>`

Adds size information:

- `int NumberOfNodes`
- `int NumberOfEdges`

### `IIndexedGraph<N, E>`

Use when nodes can be densely indexed `0..NumberOfNodes-1` and you want algorithms to run on `int` indices (often faster due to array-based data structures).

Key members:

- `int NumberOfNodes`, `int NumberOfEdges`
- `IEnumerable<int> Nodes` — enumerate node indices
- `N GetNodeLabel(int nodeIndex)` — map index to label
- `IEnumerable<int> Neighbors(int nodeIndex)` — outgoing neighbor indices
- `IEnumerable<IIndexedEdge<E>> OutEdges(int nodeIndex)`
- `IEnumerable<IIndexedEdge<E>> Edges`
- `bool ContainsEdge(int fromNode, int toNode)`
- `E GetEdgeLabel(int fromNode, int toNode)`
- `bool TryGetEdgeLabel(int fromNode, int toNode, out E edge)`

Optional/inbound members (may not be supported by all implementations):

- `IEnumerable<int> Parents(int nodeIndex)`
- `IEnumerable<IIndexedEdge<E>> InEdges(int nodeIndex)`

### `IEdge<N, E>` and `IIndexedEdge<E>`

These represent edges returned by `Edges`, `OutEdges(...)`, and `InEdges(...)`.

- `IEdge<N, E>` has `From`, `To`, and `Value`.
- `IIndexedEdge<E>` has `From`, `To`, and `Value` where `From`/`To` are `int` indices.

### `IndexedEdge<E>`

`IndexedEdge<E>` is a small struct implementing `IIndexedEdge<E>`. It is useful for adjacency-list implementations that store edges.

### `ITransposeGraph<N, E>` and `ITransposeIndexedGraph<N, E>`

If your implementation supports it, these expose:

- `Transpose()` — returns a new graph with all edges reversed.

### `ISortedGraph`

For graphs that can be topologically sorted:

- `bool IsSorted { get; }`
- `void SortTopologically()`

## Common usage patterns

### Enumerate outgoing neighbors

```csharp
using CrawfisSoftware.Collections.Graph;

public static IEnumerable<N> Depth1<N, E>(IGraph<N, E> g, N start)
{
    foreach (var n in g.Neighbors(start))
        yield return n;
}
```

### Enumerate outgoing edges (neighbor + edge label)

```csharp
using CrawfisSoftware.Collections.Graph;

public static IEnumerable<(N to, E value)> Expand<N, E>(IGraph<N, E> g, N from)
{
    foreach (var e in g.OutEdges(from))
        yield return (e.To, e.Value);
}
```

### Get an edge label safely

```csharp
using CrawfisSoftware.Collections.Graph;

public static bool TryGetCost<N>(IGraph<N, int> g, N from, N to, out int cost)
    => g.TryGetEdge(from, to, out cost);
```

### Index-based algorithm shape

```csharp
using CrawfisSoftware.Collections.Graph;

public static int OutDegree<N, E>(IIndexedGraph<N, E> g, int nodeIndex)
{
    int deg = 0;
    foreach (var _ in g.Neighbors(nodeIndex)) deg++;
    return deg;
}
```

## Notes for the provided implementations

This repository does not include the concrete `Grid`, `CompleteGraph`, or adjacency-list types themselves; it defines the interfaces that those types should implement.

The guidance below documents common expectations and how to use each style through these interfaces.

### Grid graph

Grid graphs usually represent a 2D lattice with a neighborhood rule (4-neighborhood / 8-neighborhood) and optional obstacles.

Recommended node label types:

- `ValueTuple<int,int>` (e.g., `(x, y)`) for 2D grids
- a small immutable struct for coordinates

Typical usage through `IGraph<(int x, int y), int>` (example uses `int` cost):

```csharp
using CrawfisSoftware.Collections.Graph;

public static IEnumerable<((int x, int y) to, int stepCost)> ExpandGrid(
    IGraph<(int x, int y), int> grid,
    (int x, int y) cur)
{
    foreach (var e in grid.OutEdges(cur))
        yield return (e.To, e.Value);
}
```

If your grid is dense and performance-sensitive, also implement `IIndexedGraph<(int x, int y), int>` so callers can run index-based algorithms and still recover coordinates via `GetNodeLabel(index)`.

### Complete graph (implicit edges)

A complete directed graph with *n* nodes conceptually contains all edges (often excluding self-loops, depending on your design). Because edges are implicit:

- `ContainsEdge(from, to)` is typically `true` for any valid pair
- `GetEdgeLabel(from, to)` is computed on the fly (constant or derived from nodes)
- enumerating `Edges` is `O(n^2)` — avoid for large `n`

Prefer `OutEdges(node)` / `Neighbors(node)` expansion patterns instead of materializing all edges.

### Adjacency-list graph

Adjacency lists are ideal for sparse graphs:

- `Neighbors(node)` and `OutEdges(node)` are proportional to out-degree
- `Edges` enumeration is proportional to `NumberOfEdges`
- `ContainsEdge(from, to)` may be `O(out-degree)` unless the implementation uses hashing

If the underlying representation uses indices, consider implementing `IIndexedGraph<N, E>` for performance and optionally offering a label-based adapter.

## API reference summary

- `IGraph<N, E>` / `IEdge<N, E>`
- `IFiniteGraph<N, E>`
- `IIndexedGraph<N, E>` / `IIndexedEdge<E>`
- `IndexedEdge<E>`
- `ITransposeGraph<N, E>` / `ITransposeIndexedGraph<N, E>`
- `ISortedGraph`
