using UnityEngine;

namespace Assets.Source.Component.Enemy.Interfaces
{
    public interface IEnemyDodge
    {
        void Dodge(Collision2D collision);
    }
}