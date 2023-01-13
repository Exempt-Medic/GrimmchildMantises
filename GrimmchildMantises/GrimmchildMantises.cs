using Modding;
using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using SFCore.Utils;

namespace GrimmchildMantises
{
    public class GrimmchildMantisesMod : Mod
    {
        private static GrimmchildMantisesMod? _instance;

        internal static GrimmchildMantisesMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(GrimmchildMantisesMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public GrimmchildMantisesMod() : base("GrimmchildMantises")
        {
            _instance = this;
        }

        public override void Initialize()
        {
            Log("Initializing");

            On.HutongGames.PlayMaker.Actions.StringCompare.OnEnter += NullifyGrimmchild;

            Log("Initialized");
        }

        private void NullifyGrimmchild(On.HutongGames.PlayMaker.Actions.StringCompare.orig_OnEnter orig, StringCompare self)
        {
            if (self.State.Name == "Invincible?" && self.Fsm.Name == "Attack" && self.Fsm.GameObject.name == "Enemy Damager")
            {
                GameObject go = self.Fsm.FsmComponent.FindFsmGameObjectVariable("Enemy").Value.gameObject;
                self.equalEvent =
                    (
                    (go.name.StartsWith("Bee Stinger") && go.LocateMyFSM("Bee Stinger").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Big Bee") && go.LocateMyFSM("Big Bee").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Bee Hatchling Ambient") && go.LocateMyFSM("Bee").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Zombie Hive") && go.LocateMyFSM("Hive Zombie").ActiveStateName == "Friendly?") ||
                    (go.name.StartsWith("Mantis Flyer Child") && go.LocateMyFSM("Mantis Flyer").ActiveStateName == "Lords Defeated") ||
                    (go.name.StartsWith("Mantis") && (go.LocateMyFSM("Mantis").ActiveStateName == "Friendly Idle" || go.LocateMyFSM("Mantis").ActiveStateName == "Bow"))
                    )
                    ? FsmEvent.GetFsmEvent("INVINCIBLE") : FsmEvent.GetFsmEvent("FINISHED");
                self.notEqualEvent =
                    (
                    (go.name.StartsWith("Bee Stinger") && go.LocateMyFSM("Bee Stinger").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Big Bee") && go.LocateMyFSM("Big Bee").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Bee Hatchling Ambient") && go.LocateMyFSM("Bee").GetFsmBoolVariable("Friendly").Value) ||
                    (go.name.StartsWith("Zombie Hive") && go.LocateMyFSM("Hive Zombie").ActiveStateName == "Friendly?") ||
                    (go.name.StartsWith("Mantis Flyer Child") && go.LocateMyFSM("Mantis Flyer").ActiveStateName == "Lords Defeated") ||
                    (go.name.StartsWith("Mantis") && (go.LocateMyFSM("Mantis").ActiveStateName == "Friendly Idle" || go.LocateMyFSM("Mantis").ActiveStateName == "Bow"))
                    )
                    ? FsmEvent.GetFsmEvent("INVINCIBLE") : null;
            }

            orig(self);
        }
    }
}
