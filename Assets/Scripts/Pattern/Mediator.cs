using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Helpers;
using UnityEngine;

namespace DefaultNamespace.Pattern
{
    public abstract class Mediator<T> : MonoBehaviour where T : Component, IVisitable
    { 
        readonly List<T> entities = new List<T>();

        public void Register(T entity)
        {
            if (entities.Contains(entity)) return;
            entities.Add(entity);
        }
        
        public void Deregister(T entity)
        {
            if (!entities.Contains(entity)) return;
            entities.Remove(entity);
        }

        public void Message(T source, T target, IVisitor message)
        {
            entities.FirstOrDefault(e => e.Equals(target))?.Accept(message);
        }

        public void Broadcast(T source, IVisitor message, Func<T, bool> predicate = null)
        {
            entities.Where(target => source != target && SenderConditionMet(target, predicate) && MediatorConditionMet(target))
                .ForEach(target => target.Accept(message));
        }

        bool SenderConditionMet(T target, Func<T, bool> predicate) => predicate == null || predicate(target);
        protected abstract bool MediatorConditionMet(T target);

    }
}