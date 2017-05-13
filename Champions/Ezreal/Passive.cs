using LeagueSandbox.GameServer.Logic.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSandbox.GameServer.Logic.API;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;

namespace Spells
{
    public class EzrealRisingSpellForce : GameScript
    {

        private GameScriptTimer timer;
        private ChampionStatModifier mod;
        private Buff buff;
        private int n;

        public void OnActivate(Champion owner)
        {
            ApiEventManager.OnAutoAttack.AddListener(owner, OnAutoAttack);
        }

        public void OnAutoAttack(Unit owner, Unit target)
        {
            n++;
            if (n > 5)
                n = 5;

            if (mod != null)
                owner.RemoveStatModifier(mod);
            if (timer != null)
                timer.EndTimerWithoutCallback();

            mod = new ChampionStatModifier();
            mod.AttackSpeed.PercentBonus = 0.1f * n;
            owner.AddStatModifier(mod);
            buff = ApiFunctionManager.AddBuffHUDVisual("EzrealRisingSpellForce", 6.0f, n, owner);
            timer = ApiFunctionManager.CreateTimer(6.0f, () =>
            {
                owner.RemoveStatModifier(mod);
                ApiFunctionManager.RemoveBuffHUDVisual(buff);
                mod = null;
                n = 0;
            });
        }

        public void OnDeactivate(Champion owner) { }
        public void OnStartCasting(Champion owner, Spell spell, Unit target){}
        public void OnFinishCasting(Champion owner, Spell spell, Unit target) {}
        public void ApplyEffects(Champion owner, Unit target, Spell spell, Projectile projectile) {}
        public void OnUpdate(double diff) {}
    }
}
