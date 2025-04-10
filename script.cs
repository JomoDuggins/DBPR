using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Mod
{

  //custom color
  public class CustomColor : MonoBehaviour
  {
    [Header("Custom Lightning Color")]
    [Tooltip("The color used by SpeedForceGiver or other color-driven scripts.")]
    public Color color = Color.red;

    // Optional: preview the color in the object's renderer (if it has one)
    void Start()
    {
      var renderer = GetComponent<SpriteRenderer>();
      if (renderer != null)
      {
        renderer.color = color;
      }
    }
  }

  public static class ColorExtensions
  {
    /// <summary>
    /// Convert string to Color (if defined as a static readonly field of Colors)
    /// </summary>
    /// <param name="colorName">The name of the color</param>
    /// <returns>The corresponding Color, or Color.white if not found</returns>
    public static Color ToColor(this string colorName)
    {
      var field = typeof(Colors).GetField(colorName.ToLower(), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

      if (field != null && field.FieldType == typeof(Color))
      {
        return (Color)field.GetValue(null);
      }

      Debug.LogWarning($"Color '{colorName}' not found. Returning Color.white as fallback.");
      return Color.white; // fallback color
    }
  }

  public class Mod
  {

    public static Sprite RFEye = ModAPI.LoadSprite("RFlashHeadThing.png");
    public static Sprite DBEye = ModAPI.LoadSprite("DabiHead.png");
    public static Sprite BobEye = ModAPI.LoadSprite("BobHead.png");
    public static Sprite BandiEye = ModAPI.LoadSprite("BandiHead.png");
    public static Sprite MushyEye = ModAPI.LoadSprite("MushyHead.png");

    public static Sprite SEye = ModAPI.LoadSprite("SavitarHead.png");
    public static Sprite SU = ModAPI.LoadSprite("SavitarSkinU.png");
    public static Sprite SM = ModAPI.LoadSprite("SavitarSkinM.png");
    public static Sprite SL = ModAPI.LoadSprite("SavitarSkinL.png");

    public static Sprite Null = ModAPI.LoadSprite("nothing");

    public static AudioClip VibratingLoop = ModAPI.LoadSound("VibrationLoop.wav");

    public static int SpeedForceCount = 0;
    public static float TimeScaleMultiplier = 1f;

    public static string ModTag = "<color=#FF6200>[REDUX FLASH MOD] <color=white>";
    public static string SecretTag = "<color=#FFD500>[REDUX Secret] <color=white>";

    public static DialogButton MakeColorButton(string colorName, GameObject gameObject)
    {
      return new DialogButton("<b><size=\"20%\">" + colorName + "</size></b>", true, () =>
      {
        gameObject.GetComponent<SpeedForceGiver>().CustomColor = colorName.ToColor();
      });
    }

    public static void Main()
    {
      CategoryBuilder.Create("THE FLASH MOD REDUX", "THE FLASH MOD REDUX", ModAPI.LoadSprite("Cthumb.png"));

      ModAPI.Register(new Modification()
      {
        OriginalItem = ModAPI.FindSpawnable("Rod"),
        NameOverride = ModTag + "Speed Force Giver (REDUX)",
        NameToOrderByOverride = "!01",
        DescriptionOverride = "Adds the Speed Force to any entity, making them able to use Flash's Powers",
        CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"),
        ThumbnailOverride = ModAPI.LoadSprite("SpeedForceGiverThumb.png"),
        AfterSpawn = (Instance) =>
        {
          Instance.gameObject.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("SpeedForceGiver.png");
          Instance.gameObject.FixColliders();
          Instance.gameObject.AddComponent<SpeedForceGiver>();

          // Speed Customization Button
          Instance.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
                new ContextMenuButton("CustomizeSpeed", "Customize Speed", "Customize Speed", () =>
                {
                  DialogBoxManager.Dialog("<b>Maximum Slowmotion Chooser</b>",
                   new DialogButton("<b><size=50%>20%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 5f),
                   new DialogButton("<b><size=50%>10%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 10f),
                   new DialogButton("<b><size=50%>5%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 20f),
                   new DialogButton("<b><size=50%>2%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 50f),
                   new DialogButton("<b><size=50%>1%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 100f),
                   new DialogButton("<b><size=50%>0.5%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 200f),
                   new DialogButton("<b><size=50%>0.2%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 500f),
                   new DialogButton("<b><size=50%>0.1%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 1000f),
                   new DialogButton("<b><size=50%>0.01%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 10000f),
                   new DialogButton("<b><size=50%>0.001%</size></b>", true, () => Instance.gameObject.GetComponent<SpeedForceGiver>().MaxSpeedLevel = 100000f)
                 );
                })
              );

          // Color Customization Button
          Instance.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
            new ContextMenuButton("CustomizeColor", "Customize Color", "Customize Color", () =>
            {
              DialogBoxManager.Dialog("<b>Customize Lighting Color</b>",
                Mod.MakeColorButton("Red", Instance.gameObject),
                Mod.MakeColorButton("DarkOrange", Instance.gameObject),
                Mod.MakeColorButton("GoldenRod", Instance.gameObject),
                Mod.MakeColorButton("Green", Instance.gameObject),
                Mod.MakeColorButton("Actualblue", Instance.gameObject),
                Mod.MakeColorButton("Purple", Instance.gameObject),
                Mod.MakeColorButton("White", Instance.gameObject),
                Mod.MakeColorButton("DarkSlateGray", Instance.gameObject),
                Mod.MakeColorButton("HotPink", Instance.gameObject),
                Mod.MakeColorButton("midnightblue", Instance.gameObject),
                Mod.MakeColorButton("Crimson", Instance.gameObject)
              );
            })
          );

        }
      });

      ModAPI.Register(
        new Modification()
        {
          OriginalItem = ModAPI.FindSpawnable("Human"),
          NameOverride = ModTag + " Flash (REDUX)",
          NameToOrderByOverride = "01",
          DescriptionOverride = "The Fastest man alive!\\nPress F on his Head to activate Slowmotion\\nOpen the Conext Menu on the Head for Slowmotion options",
          CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"),
          ThumbnailOverride = ModAPI.LoadSprite("FlashThumb.png"),
          AfterSpawn = (Instance) =>
             {
               var skin = ModAPI.LoadTexture("FlashSkin.png");
               var flesh = ModAPI.LoadTexture("flesh layer.png");
               var bone = ModAPI.LoadTexture("bone layer.png");
               var person = Instance.GetComponent<PersonBehaviour>();
               var head = Instance.transform.Find("Head");

               head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 1000000f;
               Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
               person.SetBodyTextures(skin, flesh, bone, 1);

               foreach (LimbBehaviour limb in person.Limbs)
               {
                 if (limb.name == "Head")
                 {
                   // Optional extra head logic
                 }
               }

               foreach (var body in person.Limbs)
               {
                 body.BaseStrength *= 1.2f;
                 body.Health *= 500f;
                 body.InitialHealth *= 500f;
                 body.BreakingThreshold *= 500f;
                 body.SkinMaterialHandler.intensityMultiplier = 0.2f;
               }
             }
        });

      //Reverse Flash
      ModAPI.Register(
        new Modification()
        {
          OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
          NameOverride = ModTag + "Reverse Flash (REDUX)", //new item name with a suffix to assure it is globally unique
          NameToOrderByOverride = "02",
          DescriptionOverride = "Flash Biggest Rival!!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
          CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
          ThumbnailOverride = ModAPI.LoadSprite("RFlashThumb.png"), //new item thumbnail (relative path)
          AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
          {
            AudioSource spawn = Instance.AddComponent<AudioSource>();
            spawn.minDistance = 1;
            spawn.maxDistance = 1;
            spawn.volume = 1;
            AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
            spawn.clip = spawnclip;

            var skin = ModAPI.LoadTexture("RFlashSkin.png");
            var flesh = ModAPI.LoadTexture("flesh layer.png");
            var bone = ModAPI.LoadTexture("bone layer.png");
            var person = Instance.GetComponent<PersonBehaviour>();
            var head = Instance.transform.Find("Head");

            var rfeyeObject = new GameObject("redeye");
            rfeyeObject.transform.SetParent(head);
            rfeyeObject.transform.localPosition = new Vector3(0f, 0f);
            rfeyeObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rfeyeObject.transform.localScale = new Vector3(1f, 1f);

            var rfeyeSprite = rfeyeObject.AddComponent<SpriteRenderer>();
            rfeyeSprite.sprite = Mod.Null;
            rfeyeSprite.sortingLayerName = "Bubbles";
            rfeyeSprite.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

            head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000f;
            head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(1f, 0f, 0f);

            Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
            person.SetBodyTextures(skin, flesh, bone, 1);

            foreach (LimbBehaviour limb in person.Limbs)
            {
              if (limb.name == "Head")
              {
                GameObject HeadThing = new GameObject("HeadThing");
                HeadThing.transform.SetParent(limb.gameObject.transform);
                HeadThing.transform.localPosition = new Vector3(0f, 0f);
                HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                HeadThing.transform.localScale = new Vector3(1f, 1f);
                HeadThing.AddComponent<SpriteRenderer>();

                SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = ModAPI.LoadSprite("RFlashHeadThing.png");
                spriteRenderer.sortingLayerName = "Foreground";
                spriteRenderer.sortingOrder += 1;
                spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
                  new ContextMenuButton(
                    "<color=#FF6200>--Negative Speedforce Eye--",
                    "<color=#FF6200>--Negative Speedforce Eye--",
                    "<color=#FF6200>--Negative Speedforce Eye--",
                    new UnityAction[1] {
                      (UnityAction) (() => {})
                      }
                  )
                );
                head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
                  new ContextMenuButton(
                    "<color=white>On",
                    "<color=white>On",
                    "<color=white>On",
                    new UnityAction[1] {
                      (UnityAction) (() => {
                        spriteRenderer.sprite = Mod.RFEye;
                        spawn.Play();

                        var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                        var particle = impact.GetComponent<ParticleSystem>();
                        var mainModule = particle.main;
                        mainModule.startSize = 1f;
                      })
                      }
                  )
                );


                head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
                  new ContextMenuButton(
                    "<color=white>Off",
                    "<color=white>Off",
                    "<color=white>Off",
                    new UnityAction[1] {
                      (UnityAction) (() => {
                        spriteRenderer.sprite = Mod.Null;
                        spawn.Play();
                        var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                        var particle = impact.GetComponent<ParticleSystem>();

                        var mainModule = particle.main;
                        mainModule.startSize = 1f;
                      })
                      }
                  )
                );

              }
            }

            head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(
              new ContextMenuButton(
                "<color=#FF6200>----",
                "<color=#FF6200>----",
                "<color=#FF6200>----",
                new UnityAction[1] {
                  (UnityAction) (() => { })
                  }
              )
            );

            foreach (var body in person.Limbs)
            {
              body.BaseStrength *= 1.2f;
              body.Health *= 500f;
              body.InitialHealth *= 500f;
              body.BreakingThreshold *= 500f;
              body.SkinMaterialHandler.intensityMultiplier = 0.2f;
            }
          }
        }
      );

      //Zoom
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Zoom (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "03",
           DescriptionOverride = "The days of The Flash protecting the city are over.\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("ZoomThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("ZoomSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 1000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(0.55f, 0.55f, 1f, 0.02f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("ZHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;
                }
              }
         }
         );

      //Godspeed
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Godspeed (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "04",
           DescriptionOverride = "There can only be one god of speed, Flash.\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("GodspeedThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("GodspeedSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 1000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(1f, 1f, 1f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(1f, 1f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("GodspeedHeadThing.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;
                }
              }
         }
         );

      //Black Flash
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Black Flash (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "05",
           DescriptionOverride = "The Speedsters version of Death, he comes for you and your speed...\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("BlackFlashThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("BlackFlashSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 1000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(1f, 0f, 0f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("ZHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;
                }
              }
         }
         );

      //Savitar
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Android"), //item to derive from
           NameOverride = ModTag + "Savitar (CUSTOM REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "06",
           DescriptionOverride = "The God of speed.\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("SavitarThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("ON.wav");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("SavitarSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");
                var upper = Instance.transform.Find("Body").Find("UpperBody");
                var middle = Instance.transform.Find("Body").Find("MiddleBody");
                var lower = Instance.transform.Find("Body").Find("LowerBody");
                var uarmf = Instance.transform.Find("FrontArm").Find("UpperArmFront");
                var uarmb = Instance.transform.Find("BackArm").Find("UpperArm");
                var larmf = Instance.transform.Find("FrontArm").Find("LowerArmFront");
                var larmb = Instance.transform.Find("BackArm").Find("LowerArm");
                var ulegf = Instance.transform.Find("FrontLeg").Find("UpperLegFront");
                var ulegb = Instance.transform.Find("BackLeg").Find("UpperLeg");
                var llegf = Instance.transform.Find("FrontLeg").Find("LowerLegFront");
                var llegb = Instance.transform.Find("BackLeg").Find("LowerLeg");
                var footf = Instance.transform.Find("FrontLeg").Find("FootFront");
                var footb = Instance.transform.Find("BackLeg").Find("Foot");

                upper.transform.localScale = new Vector3(1.05f, 1f);
                middle.transform.localScale = new Vector3(1.05f, 1f);
                lower.transform.localScale = new Vector3(1f, 1f);
                uarmf.transform.localScale = new Vector3(1.15f, 1f);
                uarmb.transform.localScale = new Vector3(1.15f, 1f);
                larmf.transform.localScale = new Vector3(1f, 1f);
                larmb.transform.localScale = new Vector3(1f, 1f);
                ulegf.transform.localScale = new Vector3(1.15f, 1f);
                ulegb.transform.localScale = new Vector3(1.15f, 1f);
                llegf.transform.localScale = new Vector3(1f, 1f);
                llegb.transform.localScale = new Vector3(1f, 1f);

                GameObject UpperThing = new GameObject("UpperThing");
                UpperThing.transform.SetParent(upper.gameObject.transform);
                UpperThing.transform.localPosition = new Vector3(0f, 0f);
                UpperThing.transform.localScale = new Vector3(1f, 1f);
                UpperThing.AddComponent<SpriteRenderer>();
                SpriteRenderer UpperRenderer = UpperThing.GetComponent<SpriteRenderer>();
                UpperRenderer.sprite = ModAPI.LoadSprite("SavitarSkinU.png");
                UpperRenderer.sortingLayerName = "Foreground";
                UpperRenderer.sortingOrder += 1;
                UpperRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                GameObject MiddleThing = new GameObject("MiddleThing");
                MiddleThing.transform.SetParent(middle.gameObject.transform);
                MiddleThing.transform.localPosition = new Vector3(0f, 0f);
                MiddleThing.transform.localScale = new Vector3(1f, 1f);
                MiddleThing.AddComponent<SpriteRenderer>();
                SpriteRenderer MiddleRenderer = MiddleThing.GetComponent<SpriteRenderer>();
                MiddleRenderer.sprite = ModAPI.LoadSprite("SavitarSkinM.png");
                MiddleRenderer.sortingLayerName = "Foreground";
                MiddleRenderer.sortingOrder += 1;
                MiddleRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                GameObject LowerThing = new GameObject("LowerThing");
                LowerThing.transform.SetParent(lower.gameObject.transform);
                LowerThing.transform.localPosition = new Vector3(0f, 0f);
                LowerThing.transform.localScale = new Vector3(1f, 1f);
                LowerThing.AddComponent<SpriteRenderer>();
                SpriteRenderer LowerRenderer = LowerThing.GetComponent<SpriteRenderer>();
                LowerRenderer.sprite = ModAPI.LoadSprite("SavitarSkinL.png");
                LowerRenderer.sortingLayerName = "Foreground";
                LowerRenderer.sortingOrder += 1;
                LowerRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                GameObject HeadThing = new GameObject("HeadThing");
                HeadThing.transform.SetParent(head.gameObject.transform);
                HeadThing.transform.localPosition = new Vector3(0f, 0f);
                HeadThing.transform.localScale = new Vector3(1f, 1f);
                HeadThing.AddComponent<SpriteRenderer>();
                SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = ModAPI.LoadSprite("SavitarHead.png");
                spriteRenderer.sortingLayerName = "Foreground";
                spriteRenderer.sortingOrder += 1;
                spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(0.247f, 0.467f, 1f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (var body in person.Limbs)
                {
                  body.transform.root.localScale *= 1.0025f;
                }

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {

                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;

                  head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Power--", "<color=#FF6200>--Power--", "<color=#FF6200>--Power--", new UnityAction[1]
            {
    (UnityAction) (() =>
    {

    })
        }));

                  head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
            {
    (UnityAction) (() =>
    {

            body.Person.Consciousness = 500f;
            spriteRenderer.sprite = Mod.SEye;
            LowerRenderer.sprite = Mod.SL;
            MiddleRenderer.sprite = Mod.SM;
            UpperRenderer.sprite = Mod.SU;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", middle.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
    })
        }));

                  head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
            {
    (UnityAction) (() =>
    {

            body.Person.Consciousness = 0f;
            spriteRenderer.sprite = Mod.Null;
            LowerRenderer.sprite = Mod.Null;
            MiddleRenderer.sprite = Mod.Null;
            UpperRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", middle.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
    })
        }));

                  head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
            {
    (UnityAction) (() =>
    {

    })
        }));
                }
              }
         }
         );
      //Harrison Wells Reverse Flash
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Harrison Wells [RF](REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "07",
           DescriptionOverride = "Flash Biggest Rival!!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("RWellsThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("RWellsSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");
                head.transform.localScale = new Vector3(1f, 1f);




                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(1f, 0f, 0f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {

                    GameObject Head2Thing = new GameObject("HeadThing");
                    Head2Thing.transform.SetParent(limb.gameObject.transform);
                    Head2Thing.transform.localPosition = new Vector3(0f, 0f);
                    Head2Thing.transform.rotation = limb.gameObject.transform.rotation;
                    Head2Thing.transform.localScale = new Vector3(1f, 1f);
                    Head2Thing.AddComponent<SpriteRenderer>();
                    SpriteRenderer sprite2Renderer = Head2Thing.GetComponent<SpriteRenderer>();
                    sprite2Renderer.sprite = ModAPI.LoadSprite("RWellsHead.png");
                    sprite2Renderer.sortingLayerName = "Foreground";
                    sprite2Renderer.sortingOrder += 1;

                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(1f, 1f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("RFlashHeadThing.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sprite = Mod.Null;
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Negative Speedforce Eye--", "<color=#FF6200>--Negative Speedforce Eye--", "<color=#FF6200>--Negative Speedforce Eye--", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.RFEye;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;
                }
              }
         }
         );

      //Barry Allen Test Suit
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Barry Allen Test Suit (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "08",
           DescriptionOverride = "The Fastest man alive!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("Flash2Thumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("Flash2Skin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.GetComponent<LimbBehaviour>().IsAndroid = true;
                head.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("AndroidArmour");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 1000f;

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(1f, 1f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("FlashHeadThing.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );

      //Cisco
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Cisco Ramon (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "09",
           DescriptionOverride = "The world's (second) best hacker alive!", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("CiscoThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("CiscoSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");


                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(1f, 1f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("CiscoHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );

      //Caitlin
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = ModTag + "Caitlin Snow (REDUX)", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "10",
           DescriptionOverride = "Cisco's scientific sidekick!", //new item description
           CategoryOverride = ModAPI.FindCategory("THE FLASH MOD REDUX"), //new item category
           ThumbnailOverride = ModAPI.LoadSprite("CaitlinThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {
                var skin = ModAPI.LoadTexture("CaitlinSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");
                var uarmf = Instance.transform.Find("FrontArm").Find("UpperArmFront");
                var uarmb = Instance.transform.Find("BackArm").Find("UpperArm");
                var larmf = Instance.transform.Find("FrontArm").Find("LowerArmFront");
                var larmb = Instance.transform.Find("BackArm").Find("LowerArm");

                var ulegf = Instance.transform.Find("FrontLeg").Find("UpperLegFront");
                var ulegb = Instance.transform.Find("BackLeg").Find("UpperLeg");
                var llegf = Instance.transform.Find("FrontLeg").Find("LowerLegFront");
                var llegb = Instance.transform.Find("BackLeg").Find("LowerLeg");

                ulegf.transform.localScale = new Vector3(0.9f, 1f);
                ulegb.transform.localScale = new Vector3(0.9f, 1f);
                llegf.transform.localScale = new Vector3(0.9f, 1f);
                llegb.transform.localScale = new Vector3(0.9f, 1f);

                uarmf.transform.localScale = new Vector3(0.9f, 1f);
                uarmb.transform.localScale = new Vector3(0.9f, 1f);
                larmf.transform.localScale = new Vector3(0.9f, 1f);
                larmb.transform.localScale = new Vector3(0.9f, 1f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(1f, 1f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("CaitlinHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );
      //Dabilast
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = SecretTag + "Dabilast", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "01",
           DescriptionOverride = "The Fastest moderator alive!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           ThumbnailOverride = ModAPI.LoadSprite("DabiThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("DabiSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(0f, 0f, 1f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("DabiHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.DBEye;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));


                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );

      //Bob
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = SecretTag + "Bob", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "02",
           DescriptionOverride = "The Fastest mod creator alive!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           ThumbnailOverride = ModAPI.LoadSprite("BobThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("BobSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(0f, 0.459f, 0.659f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("BobHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.BobEye;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));


                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );

      //Bandi
      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = SecretTag + "Bandi", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "03",
           DescriptionOverride = "The Fastest youtuber alive!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           ThumbnailOverride = ModAPI.LoadSprite("BandiThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("BandiSkin.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(0f, 0.51f, 0.392f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("BandiHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.BandiEye;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));


                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );

      ModAPI.Register(
         new Modification()
         {
           OriginalItem = ModAPI.FindSpawnable("Human"), //item to derive from
           NameOverride = SecretTag + "MushyPolter", //new item name with a suffix to assure it is globally unique
           NameToOrderByOverride = "04",
           DescriptionOverride = "The Fastest youtuber alive!\nPress F on his Head to activate Slowmotion\nOpen the Conext Menu on the Head for Slowmotion options", //new item description
           ThumbnailOverride = ModAPI.LoadSprite("MushyThumb.png"), //new item thumbnail (relative path)
           AfterSpawn = (Instance) => //all code in the AfterSpawn delegate will be executed when the item is spawned
              {

                AudioSource spawn = Instance.AddComponent<AudioSource>();
                spawn.minDistance = 1;
                spawn.maxDistance = 1;
                spawn.volume = 1;
                AudioClip spawnclip = ModAPI.LoadSound("Activate.mp3");
                spawn.clip = spawnclip;

                var skin = ModAPI.LoadTexture("MushySkin.png");
                var skin2 = ModAPI.LoadTexture("MushySkin2.png");
                var flesh = ModAPI.LoadTexture("flesh layer.png");
                var bone = ModAPI.LoadTexture("bone layer.png");
                var person = Instance.GetComponent<PersonBehaviour>();

                var head = Instance.transform.Find("Head");

                head.gameObject.AddComponent<SlowTimeBehaviour>().MaxSpeedLevel = 10000000f;
                head.gameObject.GetComponent<SlowTimeBehaviour>().color = new Color(1f, 0.314f, 0f);

                Instance.GetComponent<PersonBehaviour>().SetBodyTextures(skin, flesh, bone);
                person.SetBodyTextures(skin2, flesh, bone, 1);

                foreach (LimbBehaviour limb in person.Limbs)
                {
                  if (limb.name == "Head")
                  {
                    GameObject HeadThing = new GameObject("HeadThing");
                    HeadThing.transform.SetParent(limb.gameObject.transform);
                    HeadThing.transform.localPosition = new Vector3(0f, 0f);
                    HeadThing.transform.rotation = limb.gameObject.transform.rotation;
                    HeadThing.transform.localScale = new Vector3(0.25f, 0.25f);
                    HeadThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = HeadThing.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = ModAPI.LoadSprite("MushyHead.png");
                    spriteRenderer.sortingLayerName = "Foreground";
                    spriteRenderer.sortingOrder += 1;
                    spriteRenderer.GetComponent<SpriteRenderer>().sharedMaterial = ModAPI.FindMaterial("VeryBright");

                    GameObject HairThing = new GameObject("HeadThing");
                    HairThing.transform.SetParent(limb.gameObject.transform);
                    HairThing.transform.localPosition = new Vector3(0f, 0f);
                    HairThing.transform.rotation = limb.gameObject.transform.rotation;
                    HairThing.transform.localScale = new Vector3(1f, 1f);
                    HairThing.AddComponent<SpriteRenderer>();
                    SpriteRenderer sprite2Renderer = HairThing.GetComponent<SpriteRenderer>();
                    sprite2Renderer.sprite = ModAPI.LoadSprite("MushyHair.png");
                    sprite2Renderer.sortingLayerName = "Foreground";
                    sprite2Renderer.sortingOrder += 1;

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", "<color=#FF6200>--Inbalanced Speedforce Eye--", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>On", "<color=white>On", "<color=white>On", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.MushyEye;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
         person.SetBodyTextures(skin2);

    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=white>Off", "<color=white>Off", "<color=white>Off", new UnityAction[1]
              {
    (UnityAction) (() =>
    {
            spriteRenderer.sprite = Mod.Null;
spawn.Play();
                var impact = ModAPI.CreateParticleEffect("Vapor", head.transform.position);
                var particle = impact.GetComponent<ParticleSystem>();

                var mainModule = particle.main;
                mainModule.startSize = 1f;
         person.SetBodyTextures(skin);
    })
        }));

                    head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("<color=#FF6200>----", "<color=#FF6200>----", "<color=#FF6200>----", new UnityAction[1]
              {
    (UnityAction) (() =>
    {

    })
        }));


                  }
                }

                foreach (var body in person.Limbs)
                {
                  body.BaseStrength *= 1.2f;
                  body.Health *= 500f;
                  body.InitialHealth *= 500f;
                  body.BreakingThreshold *= 500f;
                  body.SkinMaterialHandler.intensityMultiplier = 0.2f;


                }
              }
         }
         );
      //
      //
      //
      //
      //
      //
      //
      //
      //
      //
      //
    }

    public class SpeedForceGiver : MonoBehaviour
    {
      public float MaxSpeedLevel = 1000000f;

      // This is the public color property that other scripts can access/set
      public Color CustomColor { get; set; } = Color.red;

      void Start()
      {
        // Attempt to auto-load color from a GameObject named "customcolor" (optional)
        GameObject customColorObject = GameObject.Find("customcolor");
        if (customColorObject != null)
        {
          var colorSource = customColorObject.GetComponent<CustomColor>();
          if (colorSource != null)
          {
            CustomColor = colorSource.color;
          }
        }

        ApplyColor();
      }

      void ApplyColor()
      {
        // Change sprite color to match CustomColor
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
          renderer.color = CustomColor;
        }
      }

      void OnCollisionEnter2D(Collision2D other)
      {
        // Apply the speed effect if valid target
        if (other.gameObject.GetComponent<LimbBehaviour>() &&
            other.gameObject.transform.root.GetComponent<PersonBehaviour>() &&
            !other.gameObject.transform.root.GetComponent<PersonBehaviour>().Limbs[0].gameObject.GetComponent<SlowTimeBehaviour>())
        {
          var limb = other.gameObject.transform.root.GetComponent<PersonBehaviour>().Limbs[0].gameObject;
          var slowTime = limb.AddComponent<SlowTimeBehaviour>();
          slowTime.MaxSpeedLevel = this.MaxSpeedLevel;
          slowTime.color = this.CustomColor;

          Destroy(this.gameObject);
        }
      }
    }

    public class HandShakePower : MonoBehaviour
    {
      public AudioSource audioSource;
      public GameObject Effect;

      public bool Blow = false;
      public bool Activated = false;

      public void Awake()
      {
        audioSource = new GameObject().gameObject.AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform, false);
        GameObject.FindObjectOfType<Global>().AddAudioSource(audioSource, false);
        audioSource.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.75f;
        audioSource.outputAudioMixerGroup = GameObject.FindObjectOfType<Global>().SoundEffects;
        audioSource.spatialBlend = 1;
        audioSource.volume = 1;
        audioSource.maxDistance = 1000;
      }

      public void Use()
      {
        if (Activated == false && Blow == true)
        {
          Activated = true;

          StartCoroutine(HandEffect());
        }

        else if (Activated == true && Blow == true)
        {
          Activated = false;
        }
      }

      public void Update()
      {
        if (Activated == true)
        {
          Effect.gameObject.transform.position = gameObject.transform.position;
          Effect.gameObject.transform.rotation = gameObject.transform.rotation;
          Effect.gameObject.transform.eulerAngles += new Vector3(0, 0, 90);
        }
      }

      public IEnumerator HandEffect()
      {
        audioSource.clip = Mod.VibratingLoop;
        audioSource.loop = true;
        audioSource.Play();

        Effect = GameObject.Instantiate(ModAPI.FindSpawnable("Brick").Prefab);
        Effect.gameObject.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
        Effect.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().MakeWeightless();
        Effect.transform.localScale = transform.localScale;
        Effect.FixColliders();
        Effect.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        Effect.gameObject.transform.position = gameObject.transform.position;
        Effect.gameObject.transform.rotation = gameObject.transform.rotation;

        var DontCollideWithUser = transform.root.gameObject.AddComponent<NoCollide>();
        DontCollideWithUser.NoCollideSetA = Effect.gameObject.GetComponentsInChildren<Collider2D>();
        DontCollideWithUser.NoCollideSetB = transform.root.gameObject.GetComponentsInChildren<Collider2D>();

        Effect.gameObject.AddComponent<VribrationBehaviour>();

        SpriteRenderer HandSprite = gameObject.GetComponent<SpriteRenderer>();

        GameObject HandEffect1 = new GameObject("HandEffect1");
        HandEffect1.transform.SetParent(transform);
        HandEffect1.transform.localPosition = new Vector3(0f, 0f);
        HandEffect1.transform.rotation = transform.rotation;
        HandEffect1.transform.localScale = new Vector3(1f, 1f);
        HandEffect1.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = HandEffect1.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.4f);
        spriteRenderer.sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect2 = new GameObject("HandEffect2");
        HandEffect2.transform.SetParent(transform);
        HandEffect2.transform.localPosition = new Vector3(0f, 0f);
        HandEffect2.transform.rotation = transform.rotation;
        HandEffect2.transform.localScale = new Vector3(1f, 1f);
        HandEffect2.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = HandEffect2.GetComponent<SpriteRenderer>();
        spriteRenderer2.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, 0.4f);
        spriteRenderer2.sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer2.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect3 = new GameObject("HandEffect3");
        HandEffect3.transform.SetParent(transform);
        HandEffect3.transform.localPosition = new Vector3(0f, 0f);
        HandEffect3.transform.rotation = transform.rotation;
        HandEffect3.transform.localScale = new Vector3(1f, 1f);
        HandEffect3.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer3 = HandEffect3.GetComponent<SpriteRenderer>();
        spriteRenderer3.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, 0.4f);
        spriteRenderer3.sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer3.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        gameObject.layer = 10;

        while (Activated == true && Blow == true)
        {
          HandEffect1.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.03f, -0.01f), 0f);
          HandEffect2.transform.localPosition = new Vector3(UnityEngine.Random.Range(0.01f, 0.03f), 0f);
          HandEffect3.transform.localPosition = new Vector3(UnityEngine.Random.Range(0.01f, -0.01f), 0f);

          HandSprite.color = new Color(HandSprite.color.r, HandSprite.color.g, HandSprite.color.b, UnityEngine.Random.Range(0.4f, 0.7f));
          spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, UnityEngine.Random.Range(0.3f, 0.5f));
          spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, UnityEngine.Random.Range(0.3f, 0.5f));
          spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, UnityEngine.Random.Range(0.3f, 0.5f));

          yield return new WaitForSeconds(0.03f);
        }

        HandSprite.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        gameObject.layer = 9;

        audioSource.Stop();
        Destroy(Effect.gameObject);
        Destroy(HandEffect1.gameObject);
        Destroy(HandEffect2.gameObject);
        Destroy(HandEffect3.gameObject);
      }

      private class VribrationBehaviour : MonoBehaviour
      {
        public void OnTriggerStay2D(Collider2D other)
        {
          if (other.gameObject.GetComponent<LimbBehaviour>() && other.gameObject.GetComponent<LimbBehaviour>().Health >= other.gameObject.GetComponent<LimbBehaviour>().InitialHealth / 80f)
          {
            other.gameObject.GetComponent<LimbBehaviour>().Health -= other.gameObject.GetComponent<LimbBehaviour>().InitialHealth / 80f;

            if (other.gameObject.GetComponent<LimbBehaviour>().HasLungs == true && other.gameObject.GetComponent<LimbBehaviour>().Health <= other.gameObject.GetComponent<LimbBehaviour>().InitialHealth * 0.8f)
            {
              other.gameObject.GetComponent<LimbBehaviour>().Health = 0f;

              other.gameObject.GetComponent<LimbBehaviour>().BreakBone();
              other.gameObject.GetComponent<LimbBehaviour>().HealBone();

              if (other.gameObject.GetComponent<LimbBehaviour>().IsAndroid == false)
              {
                var particle = ModAPI.CreateParticleEffect("BloodExplosion", other.transform.position);
              }

              else
              {
                var particle = ModAPI.CreateParticleEffect("BrokenElectronicsSpark", other.transform.position);
              }
            }

            if (other.gameObject.GetComponent<LimbBehaviour>().HasBrain == true)
            {
              other.gameObject.GetComponent<LimbBehaviour>().Health = 0f;

              if (other.gameObject.GetComponent<LimbBehaviour>().IsAndroid == false)
              {
                var particle = ModAPI.CreateParticleEffect("BloodExplosion", other.transform.position);
              }

              else
              {
                var particle = ModAPI.CreateParticleEffect("BrokenElectronicsSpark", other.transform.position);
              }
            }

            if (other.transform.root.GetComponent<PersonBehaviour>().PainLevel <= 1f)
            {
              other.transform.root.GetComponent<PersonBehaviour>().PainLevel += 0.01f;
            }
          }
        }
      }
    }

    public class IntangibilityPower : MonoBehaviour
    {
      public AudioSource audioSource;
      public GameObject Effect;

      public bool Blow = false;
      public bool Activated = false;

      public float Rand1;
      public float Rand2;
      public float Rand3;

      public void Awake()
      {
        audioSource = new GameObject().gameObject.AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform, false);
        GameObject.FindObjectOfType<Global>().AddAudioSource(audioSource, false);
        audioSource.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.75f;
        audioSource.outputAudioMixerGroup = GameObject.FindObjectOfType<Global>().SoundEffects;
        audioSource.spatialBlend = 1;
        audioSource.volume = 1;
        audioSource.maxDistance = 1000;
      }

      public void Use()
      {
        if (Activated == false && Blow == true)
        {
          Activated = true;

          StartCoroutine(Vibrate());
        }

        else if (Activated == true && Blow == true)
        {
          Activated = false;
        }
      }

      public IEnumerator Vibrate()
      {
        audioSource.clip = Mod.VibratingLoop;
        audioSource.loop = true;
        audioSource.Play();

        foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
        {
          StartCoroutine(ShakeEffect(body.gameObject));
        }

        while (Activated == true && Blow == true)
        {
          Rand1 = UnityEngine.Random.Range(-0.03f, -0.015f);
          Rand2 = UnityEngine.Random.Range(0.015f, 0.03f);
          Rand3 = UnityEngine.Random.Range(0.015f, -0.015f);

          yield return new WaitForSeconds(0.03f);
        }

        audioSource.Stop();
      }

      public IEnumerator ShakeEffect(GameObject body)
      {
        SpriteRenderer BodySprite = body.gameObject.GetComponent<SpriteRenderer>();

        GameObject HandEffect1 = new GameObject("HandEffect1");
        HandEffect1.transform.SetParent(body.transform);
        HandEffect1.transform.localPosition = new Vector3(0f, 0f);
        HandEffect1.transform.rotation = body.transform.rotation;
        HandEffect1.transform.localScale = new Vector3(1f, 1f);
        HandEffect1.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = HandEffect1.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.4f);
        spriteRenderer.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect2 = new GameObject("HandEffect2");
        HandEffect2.transform.SetParent(body.transform);
        HandEffect2.transform.localPosition = new Vector3(0f, 0f);
        HandEffect2.transform.rotation = body.transform.rotation;
        HandEffect2.transform.localScale = new Vector3(1f, 1f);
        HandEffect2.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = HandEffect2.GetComponent<SpriteRenderer>();
        spriteRenderer2.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, 0.4f);
        spriteRenderer2.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer2.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect3 = new GameObject("HandEffect3");
        HandEffect3.transform.SetParent(body.transform);
        HandEffect3.transform.localPosition = new Vector3(0f, 0f);
        HandEffect3.transform.rotation = body.transform.rotation;
        HandEffect3.transform.localScale = new Vector3(1f, 1f);
        HandEffect3.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer3 = HandEffect3.GetComponent<SpriteRenderer>();
        spriteRenderer3.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, 0.4f);
        spriteRenderer3.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer3.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        body.gameObject.layer = 10;

        while (Activated == true && Blow == true)
        {
          HandEffect1.transform.localPosition = new Vector3(Rand1, 0f);
          HandEffect2.transform.localPosition = new Vector3(Rand2, 0f);
          HandEffect3.transform.localPosition = new Vector3(Rand3, 0f);

          BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, UnityEngine.Random.Range(0.5f, 0.8f));
          spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, UnityEngine.Random.Range(0.3f, 0.5f));
          spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, UnityEngine.Random.Range(0.3f, 0.5f));
          spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, UnityEngine.Random.Range(0.3f, 0.5f));

          yield return new WaitForSeconds(0.03f);
        }

        BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, 1f);

        body.gameObject.layer = 9;

        Destroy(HandEffect1.gameObject);
        Destroy(HandEffect2.gameObject);
        Destroy(HandEffect3.gameObject);
      }
    }

    public class InfMassPunch : MonoBehaviour
    {
      public AudioSource audioSource;
      public GameObject Effect;

      public bool Blow = false;
      public bool Activated = false;

      public void Awake()
      {
        audioSource = new GameObject().gameObject.AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform, false);
        GameObject.FindObjectOfType<Global>().AddAudioSource(audioSource, false);
        audioSource.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.75f;
        audioSource.outputAudioMixerGroup = GameObject.FindObjectOfType<Global>().SoundEffects;
        audioSource.spatialBlend = 1;
        audioSource.volume = 1;
        audioSource.maxDistance = 1000;
      }

      public void Use()
      {
        if (Activated == false && Blow == true)
        {
          Activated = true;

          StartCoroutine(HandEffect());
        }

        else if (Activated == true && Blow == true)
        {
          Activated = false;
        }
      }

      public void FixedUpdate()
      {
        if (Activated == true)
        {
          Effect.gameObject.transform.position = gameObject.transform.position;
          Effect.gameObject.transform.rotation = gameObject.transform.rotation;
          Effect.transform.localScale = transform.localScale * 1.1f;
        }
      }

      public IEnumerator HandEffect()
      {
        Effect = GameObject.Instantiate(ModAPI.FindSpawnable("Brick").Prefab);
        Effect.gameObject.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
        Effect.gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Effect.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().Selectable = false;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().InitialMass = 9999999999f;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass = 9999999999f;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass = 9999999999f;
        Effect.gameObject.GetComponent<PhysicalBehaviour>().MakeWeightless();
        Effect.transform.localScale = transform.localScale * 1.1f;
        Effect.gameObject.transform.position = gameObject.transform.position;
        Effect.gameObject.transform.rotation = gameObject.transform.rotation;
        Effect.gameObject.GetComponent<Collider2D>().isTrigger = true;

        var DontCollideWithUser = transform.root.gameObject.AddComponent<NoCollide>();
        DontCollideWithUser.NoCollideSetA = Effect.gameObject.GetComponentsInChildren<Collider2D>();
        DontCollideWithUser.NoCollideSetB = transform.root.gameObject.GetComponentsInChildren<Collider2D>();

        Effect.gameObject.AddComponent<PunchBehaviour>().Hand = gameObject;

        while (Activated == true && Blow == true)
        {
          yield return null;
        }

        Destroy(Effect.gameObject);
      }

      private class PunchBehaviour : MonoBehaviour
      {
        public GameObject Hand;

        public void OnTriggerEnter2D(Collider2D other)
        {
          float RequiredSpeed;

          if (100f / Mod.TimeScaleMultiplier > 0.1f)
          {
            RequiredSpeed = 100f / Mod.TimeScaleMultiplier;
          }

          else
          {
            RequiredSpeed = 0.1f;
          }

          if (Hand.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > RequiredSpeed)
          {
            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              if (other.gameObject.GetComponent<LimbBehaviour>().IsAndroid == false)
              {
                var particle = ModAPI.CreateParticleEffect("BloodExplosion", other.gameObject.transform.position);
              }

              else
              {
                var particle = ModAPI.CreateParticleEffect("BrokenElectronicsSpark", other.gameObject.transform.position);
              }

              Destroy(other.gameObject);
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().Break();
            }
          }
        }
      }
    }

    public class SpeedClones : MonoBehaviour
    {
      public AudioSource audioSource;

      private List<GameObject> currentClones = new List<GameObject>();

      public bool Blow = false;

      public int Limit = 2;
      public int Current = 0;

      public float VisualMultiplier;
      public float OpacityMultiplier;
      public float OpacityMultiplier2;
      public float SoundMultiplier;

      public float Rand1;
      public float Rand2;
      public float Rand3;

      protected void OnDestroy()
      {
        foreach (GameObject currentClone in currentClones)
        {
          if (currentClone != null)
          {
            Destroy(currentClone);
          }
        }
      }

      public void Awake()
      {
        audioSource = new GameObject().gameObject.AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform, false);
        GameObject.FindObjectOfType<Global>().AddAudioSource(audioSource, false);
        audioSource.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.75f;
        audioSource.outputAudioMixerGroup = GameObject.FindObjectOfType<Global>().SoundEffects;
        audioSource.spatialBlend = 1;
        audioSource.volume = 1;
        audioSource.maxDistance = 1000;
      }

      public void Start()
      {
        if (Limit == 2)
        {
          VisualMultiplier = 0.025f;
          OpacityMultiplier = 0.3f;
          OpacityMultiplier2 = 0.05f;
          SoundMultiplier = 0.2f;
        }

        else if (Limit == 5)
        {
          VisualMultiplier = 0.011f;
          OpacityMultiplier = 0.12f;
          OpacityMultiplier2 = 0.015f;
          SoundMultiplier = 0.04f;
        }

        else if (Limit == 10)
        {
          VisualMultiplier = 0.004f;
          OpacityMultiplier = 0.06f;
          OpacityMultiplier2 = 0.07f;
          SoundMultiplier = 0.012f;
        }

        else if (Limit == 20)
        {
          VisualMultiplier = 0.001f;
          OpacityMultiplier = 0.03f;
          OpacityMultiplier2 = 0.04f;
          SoundMultiplier = 0.01f;
        }
      }

      public void Use()
      {
        if (Blow == true && Current < Limit)
        {
          Current += 1;

          StartCoroutine(CloneEffect());

          if (Current == 1)
          {
            foreach (var Limbs in transform.root.gameObject.GetComponent<PersonBehaviour>().Limbs)
            {
              StartCoroutine(ShakeEffect(Limbs.gameObject));
              StartCoroutine(Rands());

              audioSource.clip = Mod.VibratingLoop;
              audioSource.loop = true;
              audioSource.Play();
            }
          }
        }
      }

      public IEnumerator Rands()
      {
        while (Blow == true && Current > 0)
        {
          Rand1 = UnityEngine.Random.Range(Current * -VisualMultiplier, -0.002f);
          Rand2 = UnityEngine.Random.Range(0.002f, Current * VisualMultiplier);
          Rand3 = UnityEngine.Random.Range((Current * VisualMultiplier) / 4, (Current * -VisualMultiplier) / 4);

          audioSource.volume = Current * SoundMultiplier;

          yield return new WaitForSeconds(0.03f);
        }
      }

      public IEnumerator CloneEffect()
      {
        GameObject Clone = GameObject.Instantiate(ModAPI.FindSpawnable(transform.root.GetComponent<PersonBehaviour>().name).Prefab, transform.position, Quaternion.identity);
        CatalogBehaviour.PerformMod(ModAPI.FindSpawnable(transform.root.GetComponent<PersonBehaviour>().name), Clone);

        Clone.transform.root.transform.localScale = transform.root.transform.localScale;

        currentClones.Add(Clone);

        foreach (GameObject currentClone in currentClones)
        {
          if (currentClone != null)
          {
            var DontCollideWithUser = Clone.transform.root.gameObject.AddComponent<NoCollide>();
            DontCollideWithUser.NoCollideSetA = currentClone.transform.root.gameObject.GetComponentsInChildren<Collider2D>();
            DontCollideWithUser.NoCollideSetB = Clone.transform.root.gameObject.GetComponentsInChildren<Collider2D>();
          }
        }

        var DontCollideWithUser2 = Clone.transform.root.gameObject.AddComponent<NoCollide>();
        DontCollideWithUser2.NoCollideSetA = transform.root.gameObject.GetComponentsInChildren<Collider2D>();
        DontCollideWithUser2.NoCollideSetB = Clone.transform.root.gameObject.GetComponentsInChildren<Collider2D>();

        StartCoroutine(Effect(Clone.transform.root.GetComponent<PersonBehaviour>().Limbs[1].gameObject, Clone.gameObject));

        foreach (var Limbs in Clone.transform.root.gameObject.GetComponent<PersonBehaviour>().Limbs)
        {
          Limbs.gameObject.AddComponent<SpeedCloneBehaviour>().Clone = Clone.gameObject;

          if (transform.root.transform.localScale.x > 0)
          {
            Limbs.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * 12f);
          }

          else
          {
            Limbs.gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * 12f);
          }

          StartCoroutine(ShakeEffect(Limbs.gameObject));
        }

        while (Blow == true && Current > 0 && transform.root.GetComponent<PersonBehaviour>().Consciousness > 0.7f && transform.root.GetComponent<PersonBehaviour>().ShockLevel < 0.5f && transform.root.GetComponent<PersonBehaviour>().IsAlive())
        {
          yield return null;
        }

        Current = 0;

        audioSource.Stop();

        foreach (GameObject currentClone in currentClones)
        {
          if (currentClone != null)
          {
            Destroy(currentClone);
          }
        }
      }

      public IEnumerator Effect(GameObject Clone, GameObject Clone2)
      {
        AudioSource audioSource2 = new GameObject().gameObject.AddComponent<AudioSource>();
        audioSource2.transform.SetParent(Clone.transform, false);
        GameObject.FindObjectOfType<Global>().AddAudioSource(audioSource2, false);
        audioSource2.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.75f;
        audioSource2.outputAudioMixerGroup = GameObject.FindObjectOfType<Global>().SoundEffects;
        audioSource2.spatialBlend = 1;
        audioSource2.volume = 1;
        audioSource2.maxDistance = 1000;

        audioSource2.clip = Mod.VibratingLoop;
        audioSource2.loop = true;
        audioSource2.Play();

        while (Blow == true && Clone2.gameObject != null)
        {
          audioSource2.volume = Current * SoundMultiplier;

          yield return null;
        }

        Current -= 1;

        audioSource2.Stop();

        Destroy(Clone.gameObject);
      }

      public IEnumerator ShakeEffect(GameObject body)
      {
        SpriteRenderer BodySprite = body.gameObject.GetComponent<SpriteRenderer>();

        GameObject HandEffect1 = new GameObject("HandEffect1");
        HandEffect1.transform.SetParent(body.transform);
        HandEffect1.transform.localPosition = new Vector3(0f, 0f);
        HandEffect1.transform.rotation = body.transform.rotation;
        HandEffect1.transform.localScale = new Vector3(1f, 1f);
        HandEffect1.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = HandEffect1.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.4f);
        spriteRenderer.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect2 = new GameObject("HandEffect2");
        HandEffect2.transform.SetParent(body.transform);
        HandEffect2.transform.localPosition = new Vector3(0f, 0f);
        HandEffect2.transform.rotation = body.transform.rotation;
        HandEffect2.transform.localScale = new Vector3(1f, 1f);
        HandEffect2.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = HandEffect2.GetComponent<SpriteRenderer>();
        spriteRenderer2.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, 0.4f);
        spriteRenderer2.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer2.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        GameObject HandEffect3 = new GameObject("HandEffect3");
        HandEffect3.transform.SetParent(body.transform);
        HandEffect3.transform.localPosition = new Vector3(0f, 0f);
        HandEffect3.transform.rotation = body.transform.rotation;
        HandEffect3.transform.localScale = new Vector3(1f, 1f);
        HandEffect3.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer3 = HandEffect3.GetComponent<SpriteRenderer>();
        spriteRenderer3.sprite = body.gameObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, 0.4f);
        spriteRenderer3.sortingLayerName = body.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        spriteRenderer3.sortingOrder = body.gameObject.GetComponent<SpriteRenderer>().sortingOrder;

        while (Blow == true && Current > 0)
        {
          HandEffect1.transform.localPosition = new Vector3(Rand1, 0f);
          HandEffect2.transform.localPosition = new Vector3(Rand2, 0f);
          HandEffect3.transform.localPosition = new Vector3(Rand3, 0f);

          BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, UnityEngine.Random.Range(0.8f - OpacityMultiplier, 1f - OpacityMultiplier));
          spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, UnityEngine.Random.Range(0.25f - OpacityMultiplier2, 0.5f - OpacityMultiplier2));
          spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, UnityEngine.Random.Range(0.25f - OpacityMultiplier2, 0.5f - OpacityMultiplier2));
          spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, UnityEngine.Random.Range(0.25f - OpacityMultiplier2, 0.5f - OpacityMultiplier2));

          yield return new WaitForSeconds(0.03f);
        }

        BodySprite.color = new Color(BodySprite.color.r, BodySprite.color.g, BodySprite.color.b, 1f);

        Destroy(HandEffect1.gameObject);
        Destroy(HandEffect2.gameObject);
        Destroy(HandEffect3.gameObject);
      }

      public class SpeedCloneBehaviour : MonoBehaviour
      {
        public GameObject Clone;

        protected void OnDestroy()
        {
          Destroy(Clone.gameObject);
        }

        public void Shot(Shot shot)
        {
          Destroy(gameObject);
        }

        public void Stabbed(Stabbing stab)
        {
          Destroy(gameObject);
        }

        public void Slice()
        {
          Destroy(gameObject);
        }

        public void Update()
        {
          if (gameObject.GetComponent<LimbBehaviour>().Health == 0f || gameObject.GetComponent<LimbBehaviour>().Broken == true || transform.root.GetComponent<PersonBehaviour>().Consciousness < 0.7f || transform.root.GetComponent<PersonBehaviour>().ShockLevel > 0.5f || !transform.root.GetComponent<PersonBehaviour>().IsAlive())
          {
            Destroy(gameObject);
          }
        }
      }
    }

    public class SlowTimeBehaviour : MonoBehaviour
    {
      public GameObject SlowTimeThing;
      public Color color = new Color(1f, 0.9f, 0f);

      public bool EffectBlow = true;

      public bool Blow = false;
      public float TimeScaleMultiplier = 5f;
      public float SpeedLevel = 5f;
      public float MaxSpeedLevel = 10f;

      protected void OnDestroy()
      {
        Destroy(SlowTimeThing.gameObject);
      }

      public void Use()
      {
        if (Blow == false)
        {
          Blow = true;

          Mod.SpeedForceCount += 1;

          if (Mod.TimeScaleMultiplier < TimeScaleMultiplier)
          {
            Mod.TimeScaleMultiplier = TimeScaleMultiplier;
          }

          SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().Blow = true;
          SlowTimeThing.transform.localScale = new Vector3(6000f, 6000f);
        }
        else
        {
          Blow = false;

          Mod.SpeedForceCount -= 1;
          Mod.TimeScaleMultiplier = 1f;

          SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().Blow = false;
          SlowTimeThing.transform.localScale = new Vector3(0.01f, 0.01f);
        }
      }

      public void Update()
      {
        if (Blow == true)
        {
          if (SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().TimeScaleMultiplier != TimeScaleMultiplier)
          {
            SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
          }

          if (Mod.TimeScaleMultiplier < TimeScaleMultiplier)
          {
            Mod.TimeScaleMultiplier = TimeScaleMultiplier;
          }

          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            if (EffectBlow == true)
            {
              if (body.gameObject.GetComponent<TrailRenderer>().startColor != color)
              {
                body.gameObject.GetComponent<TrailRenderer>().startColor = new Color(color.r, color.g, color.b, 0.025f); ;
                body.gameObject.GetComponent<TrailRenderer>().endColor = new Color(color.r, color.g, color.b, 0.025f); ;
              }

              if (TimeScaleMultiplier < 20f)
              {
                body.gameObject.GetComponent<TrailRenderer>().time = 0.1f * (TimeScaleMultiplier / 2f);
              }
              else
              {
                body.gameObject.GetComponent<TrailRenderer>().time = 1f;
              }
            }
            else
            {
              if (body.gameObject.GetComponent<TrailRenderer>().startColor != new Color(0f, 0f, 0f, 0f))
              {
                body.gameObject.GetComponent<TrailRenderer>().startColor = new Color(0f, 0f, 0f, 0f);
                body.gameObject.GetComponent<TrailRenderer>().endColor = new Color(0f, 0f, 0f, 0f);
              }
            }
          }
        }
        else
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            if (body.gameObject.GetComponent<TrailRenderer>().startColor != new Color(0f, 0f, 0f, 0f))
            {
              body.gameObject.GetComponent<TrailRenderer>().startColor = new Color(0f, 0f, 0f, 0f);
              body.gameObject.GetComponent<TrailRenderer>().endColor = new Color(0f, 0f, 0f, 0f);
            }
          }
        }
      }

      public void Awake()
      {
        SlowTimeThing = GameObject.Instantiate(ModAPI.FindSpawnable("Metal Wheel").Prefab);
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().DisplayBloodDecals = false;
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().MakeWeightless();
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().InitialMass = 0f;
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass = 0f;
        SlowTimeThing.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass = 0f;
        SlowTimeThing.gameObject.GetComponent<Collider2D>().isTrigger = true;
        SlowTimeThing.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        SlowTimeThing.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Bottom";
        SlowTimeThing.transform.position = transform.position;
        SlowTimeThing.transform.localScale = new Vector3(0.01f, 0.01f);
        SlowTimeThing.gameObject.AddComponent<FreezeBehaviour>();

        var DontCollideWithUser = transform.root.gameObject.AddComponent<NoCollide>();
        DontCollideWithUser.NoCollideSetA = SlowTimeThing.transform.root.gameObject.GetComponentsInChildren<Collider2D>();
        DontCollideWithUser.NoCollideSetB = transform.root.gameObject.GetComponentsInChildren<Collider2D>();

        SlowTimeThing.gameObject.AddComponent<SlowBehaviour>();

        foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
        {
          body.gameObject.AddComponent<SlowImmunityBehaviour>();
        }
      }

      public void Start()
      {
        var head = transform.root.GetComponent<PersonBehaviour>().Limbs[0].gameObject;
        var upper = transform.root.GetComponent<PersonBehaviour>().Limbs[1].gameObject;
        var middle = transform.root.GetComponent<PersonBehaviour>().Limbs[2].gameObject;

        foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
        {
          body.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel = MaxSpeedLevel;

          body.gameObject.AddComponent<TrailRenderer>();
          body.gameObject.GetComponent<TrailRenderer>().startWidth = 0.12f;
          body.gameObject.GetComponent<TrailRenderer>().endWidth = 0f;
          body.gameObject.GetComponent<TrailRenderer>().time = 0.1f;
          body.gameObject.GetComponent<TrailRenderer>().startColor = new Color(color.r, color.g, color.b, 0.5f);
          body.gameObject.GetComponent<TrailRenderer>().endColor = new Color(color.r, color.g, color.b, 0.01f);
          body.gameObject.GetComponent<TrailRenderer>().material = ModAPI.FindMaterial("VeryBright");
          body.gameObject.GetComponent<TrailRenderer>().sortingLayerName = "Bottom";

          if (MaxSpeedLevel >= 5f)
          {
            body.GForceDamageThreshold *= MaxSpeedLevel * 2f;
            body.GForcePassoutThreshold *= MaxSpeedLevel;
          }
        }

        foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
        {
          if (MaxSpeedLevel >= 50f)
          {
            if (body.name.Contains("LowerArm"))
            {
              body.gameObject.AddComponent<HandShakePower>();

              head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("EnableHandShakePower", "Enable Hands Phase", "Enable Hands Phase", new UnityAction[1]
              {
                     (UnityAction) (() =>
                     {
                        body.gameObject.GetComponent<HandShakePower>().Blow = true;
                        })
                 }));

              head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("DisableHandShakePower", "Disable Hands Phase", "Disable Hands Phase", new UnityAction[1]
              {
                     (UnityAction) (() =>
                     {
                        body.gameObject.GetComponent<HandShakePower>().Blow = false;
                        })
                 }));
            }
          }
        }

        if (MaxSpeedLevel >= 200f)
        {
          upper.gameObject.AddComponent<IntangibilityPower>();

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("EnableIntangibilityPower", "Enable Intangibility", "Enable Intangibility", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  upper.gameObject.GetComponent<IntangibilityPower>().Blow = true;
                  })
             }));

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("DisableIntangibilityPower", "Disable Intangibility", "Disable Intangibility", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  upper.gameObject.GetComponent<IntangibilityPower>().Blow = false;
                  })
             }));
        }

        if (MaxSpeedLevel >= 500f)
        {
          middle.gameObject.AddComponent<SpeedClones>();

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("EnableSpeedClones", "Enable Speed Clones", "Enable Speed Clones", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  middle.gameObject.GetComponent<SpeedClones>().Blow = true;
                  })
             }));

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("DisableSpeedClones", "Disable Speed Clones", "Disable Speed Clones", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  middle.gameObject.GetComponent<SpeedClones>().Blow = false;
                  })
             }));
        }

        foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
        {
          if (MaxSpeedLevel >= 1000f)
          {
            if (body.name.Contains("LowerArm"))
            {
              body.gameObject.AddComponent<InfMassPunch>();

              head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("EnableInfMassPunch", "Enable Inf. Mass Punch", "Enable Inf. Mass Punch", new UnityAction[1]
              {
                     (UnityAction) (() =>
                     {
                        body.gameObject.GetComponent<InfMassPunch>().Blow = true;
                        })
                 }));

              head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("DisableInfMassPunch", "Disable Inf. Mass Punch", "Disable Inf. Mass Punch", new UnityAction[1]
              {
                     (UnityAction) (() =>
                     {
                        body.gameObject.GetComponent<InfMassPunch>().Blow = false;
                        })
                 }));
            }
          }
        }

        SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = SpeedLevel;

        head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("EnableEffect", "Enable Speed Effect", "Enable Speed Effect", new UnityAction[1]
        {
            (UnityAction) (() =>
            {
               EffectBlow = true;
               })
           }));

        head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("DisableEffect", "Disable Speed Effect", "Disable Speed Effect", new UnityAction[1]
        {
            (UnityAction) (() =>
            {
               EffectBlow = false;
               })
           }));

        if (MaxSpeedLevel >= 5f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.AddComponent<SpeedRegen>();
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode1", "20% Slowmotion", "20% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 5f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 10f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 1200f;
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode2", "10% Slowmotion", "10% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 10f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 20f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 1000f;
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode3", "5% Slowmotion", "5% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 20f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 50f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 800f;
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode4", "2% Slowmotion", "2% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 50f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 100f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 500f;
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode5", "1% Slowmotion", "1% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 100f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 200f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 350f;
          }

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode6", "0.5% Slowmotion", "0.5% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 200f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 500f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 200f;
          }

          middle.gameObject.GetComponent<SpeedClones>().Limit = 2;

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode7", "0.2% Slowmotion", "0.2% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 500f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 1000f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 150f;
          }

          middle.gameObject.GetComponent<SpeedClones>().Limit = 5;

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode8", "0.1% Slowmotion", "0.1% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 1000f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 10000f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 100f;
          }

          middle.gameObject.GetComponent<SpeedClones>().Limit = 10;

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode9", "0.01% Slowmotion", "0.01% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 10000f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 100000f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 80f;
          }

          middle.gameObject.GetComponent<SpeedClones>().Limit = 20;

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode10", "0.001% Slowmotion", "0.001% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 100000f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }

        if (MaxSpeedLevel >= 1000000f)
        {
          foreach (var body in transform.root.GetComponent<PersonBehaviour>().Limbs)
          {
            body.gameObject.GetComponent<SpeedRegen>().RegenMultiplier = 80f;
          }

          middle.gameObject.GetComponent<SpeedClones>().Limit = 20;

          head.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Mode11", "0.0001% Slowmotion", "0.0001% Slowmotion", new UnityAction[1]
          {
               (UnityAction) (() =>
               {
                  TimeScaleMultiplier = 1000000000f;

                  SlowTimeThing.gameObject.GetComponent<SlowBehaviour>().SpeedLevel = TimeScaleMultiplier;
                  })
             }));
        }
      }

      public class SlowBehaviour : MonoBehaviour
      {
        public bool Blow = false;
        public float TimeScaleMultiplier;
        public float SpeedLevel = 5f;

        /*public void Update()
        {
           if (Blow == true)
           {
              ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();

              foreach (ParticleSystem ps in particleSystems)
              {
                 if (!ps.gameObject.GetComponent<SlowedBehaviour>())
                 {
                    var mainModule = ps.main;
                    ps.gameObject.AddComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
                    ps.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier = mainModule.startLifetimeMultiplier;
                    mainModule.startLifetimeMultiplier = ps.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier * TimeScaleMultiplier;
                    }

                 else if (ps.gameObject.GetComponent<SlowedBehaviour>() && ps.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier != TimeScaleMultiplier)
                 {
                    var mainModule = ps.main;
                    mainModule.startLifetimeMultiplier = ps.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier * TimeScaleMultiplier;
                    ps.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
                    }
                 }

              IonBoltBehaviour[] ionBoltBehaviour = FindObjectsOfType<IonBoltBehaviour>();

              foreach (IonBoltBehaviour ibb in ionBoltBehaviour)
              {
                 if (!ibb.gameObject.GetComponent<SlowedBehaviour>())
                 {
                    ibb.gameObject.AddComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
                    ibb.gameObject.GetComponent<SlowedBehaviour>().OriginalSpeed = ibb.Speed;
                    ibb.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier = ibb.gameObject.GetComponent<DeleteAfterTime>().Life;
                    ibb.Speed /= TimeScaleMultiplier;
                    ibb.gameObject.GetComponent<DeleteAfterTime>().Life *= TimeScaleMultiplier;
                    }

                 else if (ibb.gameObject.GetComponent<SlowedBehaviour>() && ibb.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier != TimeScaleMultiplier)
                 {
                    ibb.Speed = ibb.gameObject.GetComponent<SlowedBehaviour>().OriginalSpeed / TimeScaleMultiplier;
                    ibb.gameObject.GetComponent<DeleteAfterTime>().Life = ibb.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier / TimeScaleMultiplier;

                    ibb.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
                    }
                 }
              }

           else
           {
              /*ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();

              foreach (ParticleSystem ps in particleSystems)
              {
                 if (ps.gameObject.GetComponent<SlowedBehaviour>())
                 {
                    var mainModule = ps.main;
                    mainModule.startLifetimeMultiplier = ps.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier;

                    Destroy(ps.gameObject.GetComponent<SlowedBehaviour>());
                    }
                 }*/

        /*IonBoltBehaviour[] ionBoltBehaviour = FindObjectsOfType<IonBoltBehaviour>();

        foreach (IonBoltBehaviour ibb in ionBoltBehaviour)
        {
           if (ibb.gameObject.GetComponent<SlowedBehaviour>())
           {
              ibb.Speed = ibb.gameObject.GetComponent<SlowedBehaviour>().OriginalSpeed;
              ibb.gameObject.GetComponent<DeleteAfterTime>().Life = ibb.gameObject.GetComponent<SlowedBehaviour>().startLifetimeMultiplier;

              Destroy(ibb.gameObject.GetComponent<SlowedBehaviour>());
              }
           }
        }
     }*/

        public void OnTriggerEnter2D(Collider2D other)
        {
          if (Blow == true && !other.gameObject.GetComponent<SlowedBehaviour>() && !other.gameObject.GetComponent<SlowImmunityBehaviour>() && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            /*if (other.gameObject.GetComponent<BlasterBehaviour>() && !other.gameObject.GetComponent<SlowedBehaviour>())
            {
               other.gameObject.AddComponent<SlowedProjectileBehaviour>().ProjID = 1;
               }*/

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier /= TimeScaleMultiplier * 2f;
              other.gameObject.GetComponent<LimbBehaviour>().GForceDamageThreshold *= TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().GForcePassoutThreshold *= TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength /= TimeScaleMultiplier;
            }

            other.gameObject.AddComponent<SlowedBehaviour>().InitialGravityScale = other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale;

            other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale = other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass;
            other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;

            if (Global.main.SlowMotion == false)
            {
              other.gameObject.GetComponent<SlowedBehaviour>().OriginalSound = other.gameObject.GetComponent<AudioSource>().pitch;
            }

            else
            {
              other.gameObject.GetComponent<SlowedBehaviour>().OriginalSound = other.gameObject.GetComponent<AudioSource>().pitch / Global.main.SlowmotionTimescale;
            }

            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass *= TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass *= TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass *= TimeScaleMultiplier;
            other.gameObject.GetComponent<Rigidbody2D>().velocity /= TimeScaleMultiplier;
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity /= TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale /= TimeScaleMultiplier * 6f;
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale /= TimeScaleMultiplier * 6f;

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce /= TimeScaleMultiplier;
            }
          }

          else if (Blow == true && !other.gameObject.GetComponent<SlowedBehaviour>() && other.gameObject.GetComponent<SlowImmunityBehaviour>() && other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel < SpeedLevel && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            other.gameObject.AddComponent<SlowedBehaviour>().InitialGravityScale = other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale;
            other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale = other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass;
            other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
            other.gameObject.GetComponent<SlowedBehaviour>().OriginalSound = other.gameObject.GetComponent<AudioSource>().pitch;

            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass *= TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass *= TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass *= TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= 1f / (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity *= 1f / (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale *= 1f / ((TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel) * 5f);
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale *= 1f / ((TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel) * 5f);

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier *= 1f / (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength *= 1f / (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce *= 1f / (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            }

            /*if (other.transform.root.GetComponent<PersonBehaviour>())
            {
               for (var i = 0; i < other.transform.root.GetComponent<PersonBehaviour>().LinkedPoses.Length; i++)
               {
                  other.transform.root.GetComponent<PersonBehaviour>().LinkedPoses[i].AnimationSpeedMultiplier = -1f / TimeScaleMultiplier;
                  }

               foreach(RagdollPose poseState in other.transform.root.GetComponent<PersonBehaviour>().LinkedPoses)
               {
                  poseState.AnimationSpeedMultiplier = -1f / TimeScaleMultiplier;
                  }
               }*/
          }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
          if (Blow == false && other.gameObject.GetComponent<SlowedBehaviour>() && !other.gameObject.GetComponent<SlowImmunityBehaviour>() && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            int parents = 0;

            foreach (Transform otherTransform in GameObject.FindObjectsOfType<Transform>())
            {
              if (otherTransform.parent == other.transform)
              {
                if (otherTransform.GetComponent<Rigidbody2D>() && otherTransform.GetComponent<PhysicalBehaviour>())
                {
                  parents++;
                }
              }
            }

            int Childs = other.gameObject.transform.childCount + parents + 1;

            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= 1.6f * (TimeScaleMultiplier / Childs);
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity *= 1.6f * (TimeScaleMultiplier / Childs);
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale = other.gameObject.GetComponent<SlowedBehaviour>().InitialGravityScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale = other.gameObject.GetComponent<SlowedBehaviour>().InitialGravityScale;

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier *= TimeScaleMultiplier * 2f;
              other.gameObject.GetComponent<LimbBehaviour>().GForceDamageThreshold /= TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().GForcePassoutThreshold /= TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength *= TimeScaleMultiplier;
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce *= TimeScaleMultiplier;
            }

            float LocalOriginalSound = other.gameObject.GetComponent<SlowedBehaviour>().OriginalSound;

            Destroy(other.gameObject.GetComponent<SlowedBehaviour>());

            other.gameObject.GetComponent<AudioSource>().pitch = LocalOriginalSound;
          }

          else if (Blow == false && other.gameObject.GetComponent<SlowedBehaviour>() && other.gameObject.GetComponent<SlowImmunityBehaviour>() && other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel < SpeedLevel && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            int parents = 0;

            foreach (Transform otherTransform in GameObject.FindObjectsOfType<Transform>())
            {
              if (otherTransform.parent == other.transform)
              {
                if (otherTransform.GetComponent<Rigidbody2D>() && otherTransform.GetComponent<PhysicalBehaviour>())
                {
                  parents++;
                }
              }
            }

            int Childs = other.gameObject.transform.childCount + parents + 1;

            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass = other.gameObject.GetComponent<SlowedBehaviour>().InitialMassScale;
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= 1.6f * ((TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel) / Childs);
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity *= 1.6f * ((TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel) / Childs);
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale = other.gameObject.GetComponent<SlowedBehaviour>().InitialGravityScale;
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale = other.gameObject.GetComponent<SlowedBehaviour>().InitialGravityScale;

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier *= (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength *= (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce *= (TimeScaleMultiplier / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            }

            float LocalOriginalSound = other.gameObject.GetComponent<SlowedBehaviour>().OriginalSound;

            Destroy(other.gameObject.GetComponent<SlowedBehaviour>());

            other.gameObject.GetComponent<AudioSource>().pitch = LocalOriginalSound;
          }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
          if (Blow == true && other.gameObject.GetComponent<SlowedBehaviour>() && other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier != TimeScaleMultiplier && !other.gameObject.GetComponent<SlowImmunityBehaviour>() && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().GForceDamageThreshold /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().GForcePassoutThreshold /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength /= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce *= other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier;
            }

            other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
          }

          else if (Blow == true && other.gameObject.GetComponent<SlowedBehaviour>() && other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier != TimeScaleMultiplier && other.gameObject.GetComponent<SlowImmunityBehaviour>() && other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel < SpeedLevel && !other.gameObject.GetComponent<FreezeBehaviour>() && other.gameObject.GetComponent<PhysicalBehaviour>() && other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
          {
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialMass *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().TrueInitialMass *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.gravityScale *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            other.gameObject.GetComponent<PhysicalBehaviour>().InitialGravityScale *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;

            if (other.gameObject.GetComponent<LimbBehaviour>())
            {
              other.gameObject.GetComponent<LimbBehaviour>().ImpactDamageMultiplier *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
              other.gameObject.GetComponent<LimbBehaviour>().BaseStrength *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            }

            if (other.gameObject.GetComponent<DestroyableBehaviour>())
            {
              other.gameObject.GetComponent<DestroyableBehaviour>().MinimumImpactForce *= (other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier / TimeScaleMultiplier) / other.gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel;
            }

            other.gameObject.GetComponent<SlowedBehaviour>().TimeScaleMultiplier = TimeScaleMultiplier;
          }
        }
      }

      public class SlowedBehaviour : MonoBehaviour
      {
        public float InitialGravityScale;
        public float InitialMassScale;
        public float OriginalSound;
        public float startLifetimeMultiplier;
        public float OriginalSpeed;
        public float Consciousness;
        public float TimeScaleMultiplier;

        public void Update()
        {
          if (!gameObject.GetComponent<SlowImmunityBehaviour>())
          {
            if (gameObject.GetComponent<AudioSource>().pitch != OriginalSound / TimeScaleMultiplier)
            {
              gameObject.GetComponent<AudioSource>().pitch = OriginalSound / TimeScaleMultiplier;
            }
          }

          else if (gameObject.GetComponent<SlowImmunityBehaviour>() && gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel < TimeScaleMultiplier)
          {
            if (gameObject.GetComponent<AudioSource>().pitch != OriginalSound / (TimeScaleMultiplier / gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel))
            {
              gameObject.GetComponent<AudioSource>().pitch = OriginalSound / (TimeScaleMultiplier / gameObject.GetComponent<SlowImmunityBehaviour>().MaxSpeedLevel);
            }
          }
        }
      }
    }

    public class SlowImmunityBehaviour : MonoBehaviour
    {
      public float SpeedLevel = 5f;
      public float MaxSpeedLevel = 5f;
    }

    public class SpeedRegen : MonoBehaviour
    {
      public float RegenMultiplier = 1500f;
      public bool BrokenBone = false;
      public bool Bleeding = false;

      public void Update()
      {
        if (gameObject.transform.root.GetComponent<PersonBehaviour>().IsAlive())
        {
          gameObject.GetComponent<LimbBehaviour>().Health += (gameObject.GetComponent<LimbBehaviour>().InitialHealth / RegenMultiplier) * Time.deltaTime;
        }

        if (gameObject.GetComponent<LimbBehaviour>().Broken == true && BrokenBone == false)
        {
          BrokenBone = true;

          StartCoroutine(StartHealBone());
        }

        if (gameObject.GetComponent<LimbBehaviour>().GetComponent<CirculationBehaviour>().BleedingPointCount > 0 && Bleeding == false)
        {
          Bleeding = true;

          StartCoroutine(StartHealBleeding());
        }
      }

      public IEnumerator StartHealBone()
      {
        yield return new WaitForSeconds(0.1f * RegenMultiplier);

        if (gameObject.transform.root.GetComponent<PersonBehaviour>().IsAlive())
        {
          gameObject.GetComponent<LimbBehaviour>().HealBone();
        }
      }

      public IEnumerator StartHealBleeding()
      {
        yield return new WaitForSeconds(0.04f * RegenMultiplier);

        if (gameObject.transform.root.GetComponent<PersonBehaviour>().IsAlive())
        {
          gameObject.GetComponent<LimbBehaviour>().HealBone();
        }
      }
    }
  }
}