using Pancake.Localization;
using Pancake.Spine;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;

namespace Pancake.SceneFlow
{
    using UnityEngine;

    public class DayComponent : MonoBehaviour
    {
        [SerializeField] private LocaleTextComponent localeTextDay;
        [SerializeField] private TextMeshProUGUI textValueReward;
        [SerializeField] private GameObject claimedObject;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageIconReward;
        [SerializeField] private SkeletonGraphic outfitGraphic;

        [SerializeField] private Sprite backgroundNormal;
        [SerializeField] private Sprite backgroundCurrent;

        public void Init(DailyRewardVariable variable, BoolDailyVariable detectNewDay)
        {
            int day = variable.Value.day + (UserData.GetCurrentWeekDailyReward() - 1) * 7;
            localeTextDay.UpdateArgs($"{day}");
            textValueReward.SetText($"+{variable.Value.amount}");

            if (variable.Value.typeReward == TypeRewardDailyReward.Coin)
            {
                textValueReward.gameObject.SetActive(true);
                if (variable.IsClaimed())
                {
                    claimedObject.SetActive(true);
                    if (outfitGraphic != null) outfitGraphic.gameObject.SetActive(false);
                    imageIconReward.gameObject.SetActive(false);
                    textValueReward.gameObject.SetActive(false);
                }
                else
                {
                    claimedObject.SetActive(false);
                    if (outfitGraphic != null) outfitGraphic.gameObject.SetActive(false);
                    textValueReward.gameObject.SetActive(true);
                    imageIconReward.gameObject.SetActive(true);
                    imageIconReward.sprite = variable.Value.icon;
                }
            }
            else
            {
                textValueReward.gameObject.SetActive(false);
                if (variable.IsClaimed())
                {
                    claimedObject.SetActive(true);
                    if (outfitGraphic != null) outfitGraphic.gameObject.SetActive(false);
                    imageIconReward.gameObject.SetActive(false);
                }
                else
                {
                    claimedObject.SetActive(false);
                    imageIconReward.gameObject.SetActive(false);
                    if (outfitGraphic != null)
                    {
                        outfitGraphic.gameObject.SetActive(true);
                        outfitGraphic.ChangeSkin(variable.Value.outfitUnit.Value.skinId);
                    }
                }
            }

            if (detectNewDay.Value)
            {
                imageBackground.sprite = variable.Value.day == (UserData.GetCurrentDayDailyReward() - 1).Max(0) ? backgroundCurrent : backgroundNormal;
            }
            else
            {
                imageBackground.sprite = variable.Value.day == UserData.GetCurrentDayDailyReward() ? backgroundCurrent : backgroundNormal;
            }
        }
    }
}