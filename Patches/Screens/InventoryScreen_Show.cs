using Comfort.Common;
using EFT;
using EFT.Quests;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPT.SinglePlayer.Utils.InRaid;
using System;
using System.Linq;
using System.Reflection;
using TaskAutomation.Helpers;
using TaskAutomation.MonoBehaviours;

namespace TaskAutomation.Patches.Screens
{
    internal class InventoryScreen_Show : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.FirstMethod(typeof(InventoryScreen), this.IsTargetMethod);
        }

        [PatchPostfix]
        private static void PatchPostfix(InventoryScreen __instance, object questController, IEftSession session)
        {
            if (RaidTimeUtil.HasRaidLoaded()
                || questController is not QuestController abstractQuestController)
                return;
            if (Globals.Debug)
                LogHelper.LogInfo($"Found abstractQuestController.");
            if (Globals.Debug)
                LogHelper.LogInfo($"Found session {session.GetType()}.");
            Singleton<UpdateMonoBehaviour>.Instance.SetSession(session);
            Singleton<UpdateMonoBehaviour>.Instance.SetAbstractQuestController(abstractQuestController);
        }

        private bool IsTargetMethod(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            return method.Name == nameof(InventoryScreen.Show)
                && parameters.Length != 1;
        }
    }
}