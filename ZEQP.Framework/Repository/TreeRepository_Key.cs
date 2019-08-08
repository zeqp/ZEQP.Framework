using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEQP.Framework
{
    public class TreeRepository<T, K> : BaseRepository<T, K>, ITreeRepository<T, K>
        where T : TreeEntity<K>
    {
        public TreeRepository(DbContext context, IMapper mapper)
            : base(context, mapper)
        {

        }

        public List<T> GetAllChildren(T parent)
        {
            return this.GetList(w => w.Path.StartsWith(parent.Path));
        }

        public Task<List<T>> GetAllChildrenAsync(T parent)
        {
            return this.GetListAsync(w => w.Path.StartsWith(parent.Path));
        }

        public override bool Update(T entity, bool save = true, List<string> props = null)
        {
            if (entity.ParentId.Equals(entity.Id))
                throw new Exception("实体的父级ID不能是自己");
            var old = this.Get(entity.Id, false);
            if (old == null) return false;
            if (entity.ParentId.Equals(old.ParentId))
                return base.Update(entity, save, props);
            if (props != null && !props.Contains(nameof(entity.ParentId)))
                return base.Update(entity, save, props);
            var children = this.GetAllChildren(entity);
            if (children.Exists(t => t.Id.Equals(entity.ParentId)))
                throw new Exception("子级不能包含父级");
            var parent = this.Get(entity.ParentId);
            parent.IsLeaf = false;
            entity.InitPath(parent);
            UpdateChildrenPath(entity, children);
            return base.Update(children, save, props);
        }
        public override async Task<bool> UpdateAsync(T entity, bool save = true, List<string> props = null)
        {
            if (entity.ParentId.Equals(entity.Id))
                throw new Exception("实体的父级ID不能是自己");
            var old = await this.GetAsync(entity.Id, false);
            if (old == null) return false;
            if (entity.ParentId.Equals(old.ParentId))
                return await base.UpdateAsync(entity, save, props);
            if (props != null && !props.Contains(nameof(entity.ParentId)))
                return await base.UpdateAsync(entity, save, props);
            var children = await this.GetAllChildrenAsync(entity);
            if (children.Exists(t => t.Id.Equals(entity.ParentId)))
                throw new Exception("子级不能包含父级");
            var parent = await this.GetAsync(entity.ParentId);
            parent.IsLeaf = false;
            entity.InitPath(parent);
            await UpdateChildrenPathAsync(entity, children);
            return await base.UpdateAsync(children, save, props);
        }

        /// <summary>
        /// 修改路径
        /// </summary>
        private void UpdateChildrenPath(T parent, List<T> children)
        {
            if (parent == null || children == null)
                return;
            var list = children.Where(t => t.ParentId.Equals(parent.Id)).ToList();
            parent.IsLeaf = list.Count == 0;
            foreach (var child in list)
            {
                child.InitPath(parent);
                UpdateChildrenPath(child, children);
            }
        }
        /// <summary>
        /// 修改路径
        /// </summary>
        private async Task UpdateChildrenPathAsync(T parent, List<T> children)
        {
            if (parent == null || children == null)
                return;
            var list = children.Where(t => t.ParentId.Equals(parent.Id)).ToList();
            parent.IsLeaf = list.Count == 0;
            foreach (var child in list)
            {
                child.InitPath(parent);
                await UpdateChildrenPathAsync(child, children);
            }
        }
    }
}
