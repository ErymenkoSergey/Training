using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Inventories
{
    public class Inventory : IEnumerable<Item>
    {
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => sizeInventory.x;
        public int Height => sizeInventory.y;
        public int Count => sizeInventory.x * sizeInventory.y;
        
        private Dictionary<Vector2Int, Item> items = new Dictionary<Vector2Int, Item>();
        private Vector2Int sizeInventory; // размер инвентаря по x и y 

        #region Constructors
        
        public Inventory(int width, int height)
        {
            sizeInventory.x = width;
            sizeInventory.y = height;
        }

        public Inventory(int width, int height, params KeyValuePair<Item, Vector2Int>[] items)
        {
            sizeInventory.x = width;
            sizeInventory.y = height;
        }

        public Inventory(int width, int height, params Item[] items)
        {
            sizeInventory.x = width;
            sizeInventory.y = height;
        }

        public Inventory(int width, int height, IEnumerable<KeyValuePair<Item, Vector2Int>> items)
        {
            sizeInventory.x = width;
            sizeInventory.y = height;
        }

        public Inventory(int width, int height, IEnumerable<Item> items)
        {
            sizeInventory.x = width;
            sizeInventory.y = height;
        }

        /// <summary>
        /// Creates new inventory 
        /// </summary>
        public Inventory(Inventory inventory)
        {
            //??
        }
        
        #endregion

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(Item item, Vector2Int position)
        {
            if (!items.ContainsKey(position))
            {
                items.Add(position, item);
                return true;
            }
            else
                return false;
        }

        public bool CanAddItem(Item item, int startX, int startY)
        {
           return CanAddItem(item, new Vector2Int(startX, startY));
        }

        /// <summary>
        /// Adds an item on a specified position
        /// </summary>
        public bool AddItem(Item item, Vector2Int position)
        {
            return CanAddItem(item, position);
        }

        public bool AddItem(Item item, int startX, int startY)
        {
            return CanAddItem(item, new Vector2Int(startX, startY));
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(Item item, out Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public bool FindFreePosition(Vector2Int size, out Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public bool FindFreePosition(int sizeX, int sizeY, out Vector2Int position)
        {
            throw new NotImplementedException();
        }

        private bool IsFreeSpace(int startX, int startY, int endX, int endY)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the specified element exists
        /// </summary>
        public bool Contains(Item item)
        {
            if (items.ContainsValue(item))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the specified position is occupied
        /// </summary>
        public bool IsOccupied(Vector2Int position)
        {
            if (items.ContainsKey(position))
                return true;
            else
                return false;
        }

        public bool IsOccupied(int x, int y)
        {
            if (items.ContainsKey(new Vector2Int(x, y)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the specified position is free
        /// </summary>
        public bool IsFree(Vector2Int position)
        {
            if (!items.ContainsKey(position))
                return true;
            else
                return false;
        }

        public bool IsFree(int x, int y)
        {
            return IsFree(new Vector2Int(x, y));
        }

        /// <summary>
        /// Removes specified item
        /// </summary>
        public bool RemoveItem(Item item)
        {
            // if (items.ContainsValue(item))
            // {
            //     // var key = items.ContainsKey()
            //     items.Remove(new Vector2Int())
            // }
            throw new NotImplementedException();
        }

        public bool RemoveItem(Item item, out Vector2Int position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool TryGetItem(Vector2Int position, out Item item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetItem(int x, int y, out Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(Item item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPositions(Item item, out Vector2Int[] positions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all items 
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            throw new NotImplementedException();
        }

        public bool MoveItem(Item item, Vector2Int position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rearranges an inventory space with max free slots 
        /// </summary>
        public void OptimizeSpace()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Iterates by all items 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Item> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies items to a specified matrix
        /// </summary>
        public void CopyTo(Item[,] matrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an inventory matrix in string format
        /// </summary>
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}