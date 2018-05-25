using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MusicMetadataUpdater.UnitTests
{
    [Obsolete("Potentially to be used for testing the the extension methods that alter DbSets.")]
    public class FakeDbSet<TEntity> : IDbSet<TEntity>//DbSet<TEntity>, IQueryable<TEntity>
        where TEntity : class
    {
        private readonly ObservableCollection<TEntity> _data;
        private readonly IQueryable<TEntity> _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public TEntity Add(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> items)
        {
            return items.Select(Add).ToList();
        }

        public TEntity Remove(TEntity item)
        {
            if (item != null)
                _data.Remove(item);
            return item;
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> items)
        {
            if (items == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in items.Select(Remove).ToList())
            {
                yield return entity;
            }
            //return entities.Select(Remove).ToList();
        }

        public TEntity Attach(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public TEntity Detach(TEntity item)
        {
            if (_data.Contains(item))
                _data.Remove(item);
            return item;
        }

        public TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}