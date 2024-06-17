# Object Pooling

Object pooling library for those who still using unsupported unity version with no object pooling library in it.
This library is imitating original unity object pooling approach.

## Example

We had to make the pool first before using it.

#### GameObjectPool.cs

```csharp
IObjectPool<GameObject> _goPool;

void Start(){
    InitializePool();
}

void InitializePool(){
    _goPool = new ObjectPool<GameObject>.Builder()
        .CreateFactory(InstantiateGO) // item creation
        .WithStartingCapacity(_poolSize) // pool default capacity
        .WithMaxCapacity(_maxPoolSize) // pool maximum capacity
        .WhenGetItem(TakeFromPool) // action when item taken
        .WhenItemReturned(ReturnedToPool) // action when item released
        .WhenItemDisposed(DisposedGO) // item disposed action
        .Build();
}

public GameObject GetObject() => _goPool.GetItem();
public void ReturnObject(GameObject obj) => _goPool.ReleaseItem(obj);
```

With the pool ready to use we could call it from anywhere.

```csharp
GameObjectPool _pool;

// Get the object from pool
var gObj = _pool.GetObject();

// Return object to  pool
_pool.ReturnObject(gObj);
```

## Thank-you Notes

This library was almost fully inspired by [Peter Cardwell-Gardner](https://github.com/thefuntastic), kindly checkout his
[unity object pool](https://github.com/thefuntastic/unity-object-pool) version.

Lastly thanks for visiting this page. And I will gladly accept any kind of feedback. You can contact me through here <nabilahkishou@gmail.com>.