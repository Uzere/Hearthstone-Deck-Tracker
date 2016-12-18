#region

using System.Linq;
using HearthDb;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using static HearthDb.Enums.GameTag;
using System;

#endregion

namespace Hearthstone_Deck_Tracker.Utility
{
	public static class WotogCounterHelper
	{
		public static Entity PlayerCthun => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.Cthun && x.Info.OriginalZone != null);
		public static Entity PlayerCthunProxy => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.NonCollectible.Neutral.Cthun);
		public static Entity PlayerYogg => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.YoggSaronHopesEnd && x.Info.OriginalZone != null);
		public static Entity PlayerArcaneGiant => Core.Game.Player.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.ArcaneGiant && x.Info.OriginalZone != null);
		public static Entity OpponentCthun => Core.Game.Opponent.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.Collectible.Neutral.Cthun);
		public static Entity OpponentCthunProxy => Core.Game.Opponent.PlayerEntities.FirstOrDefault(x => x.CardId == CardIds.NonCollectible.Neutral.Cthun);
		public static bool PlayerSeenCthun => Core.Game.PlayerEntity?.HasTag(SEEN_CTHUN) ?? false;
		public static bool OpponentSeenCthun => Core.Game.OpponentEntity?.HasTag(SEEN_CTHUN) ?? false;
		public static bool? CthunInDeck => DeckContains(CardIds.Collectible.Neutral.Cthun);
		public static bool? YoggInDeck => DeckContains(CardIds.Collectible.Neutral.YoggSaronHopesEnd);
		public static bool? ArcaneGiantInDeck => DeckContains(CardIds.Collectible.Neutral.ArcaneGiant);

		public static int PlayerMaxJadeGolem => Core.Game.Player.PlayerEntities.
					Where(x => IsJadeGolem(x) && !x.Info.Stolen).
					Max(x => (Nullable<Int32>)x.Card.Attack) ?? 0;
		public static bool PlayerSeenJade => PlayerMaxJadeGolem > 0;

		public static int OpponentMaxJadeGolem => Core.Game.Opponent.PlayerEntities.
					Where(x => IsJadeGolem(x) && !x.Info.Stolen).
					Max(x => (Nullable<Int32>)x.Card.Attack) ?? 0;
		public static bool OpponentSeenJade => OpponentMaxJadeGolem > 0;

		public static bool ShowPlayerCthunCounter => !Core.Game.IsInMenu && (Config.Instance.PlayerCthunCounter == DisplayMode.Always
					|| Config.Instance.PlayerCthunCounter == DisplayMode.Auto && PlayerSeenCthun);

		public static bool ShowPlayerSpellsCounter => !Core.Game.IsInMenu && (
			Config.Instance.PlayerSpellsCounter == DisplayMode.Always
				|| (Config.Instance.PlayerSpellsCounter == DisplayMode.Auto && YoggInDeck.HasValue && (PlayerYogg != null || YoggInDeck.Value))
				|| (Config.Instance.PlayerSpellsCounter == DisplayMode.Auto && ArcaneGiantInDeck.HasValue && (PlayerArcaneGiant != null || ArcaneGiantInDeck.Value))
			);

		public static bool ShowPlayerJadeCounter => !Core.Game.IsInMenu && (Config.Instance.PlayerJadeCounter == DisplayMode.Always
					|| Config.Instance.PlayerJadeCounter == DisplayMode.Auto && PlayerSeenJade);

		public static bool ShowOpponentCthunCounter => !Core.Game.IsInMenu && (Config.Instance.OpponentCthunCounter == DisplayMode.Always
					|| Config.Instance.OpponentCthunCounter == DisplayMode.Auto && OpponentSeenCthun);

		public static bool ShowOpponentSpellsCounter => !Core.Game.IsInMenu && Config.Instance.OpponentSpellsCounter == DisplayMode.Always;

		public static bool ShowOpponentJadeCounter => !Core.Game.IsInMenu && (Config.Instance.OpponentJadeCounter == DisplayMode.Always
					|| Config.Instance.OpponentJadeCounter == DisplayMode.Auto && OpponentSeenJade);

		private static bool? DeckContains(string cardId) => DeckList.Instance.ActiveDeck?.Cards.Any(x => x.Id == cardId);

		private static int JadeGolemIdLength = CardIds.NonCollectible.Neutral.JadeGolem1.Length;
		private static string JadeGolemIdPrefix = CardIds.NonCollectible.Neutral.JadeGolem1.Substring(0, JadeGolemIdLength - 2);
		private static bool IsJadeGolem(Entity x) => x.CardId != null && x.CardId.Length == JadeGolemIdLength &&
					x.CardId.Substring(0, JadeGolemIdLength - 2) == JadeGolemIdPrefix;
	}
}