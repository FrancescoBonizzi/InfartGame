using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Infart.Assets
{
    public class AssetsLoader
    {
        private readonly ContentManager _contentManager;

        public Texture2D Textures { get; private set; }
        public Texture2D TexturesBuildingsBack { get; private set; }
        public Texture2D TexturesBuildingsMid { get; private set; }
        public Texture2D TexturesBuildingsGround { get; private set; }
        public List<Rectangle> PlayerRunRects { get; private set; }
        public List<Rectangle> PlayerIdleRects { get; private set; }
        public List<Rectangle> PlayerFallRects { get; private set; }
        public List<Rectangle> PlayerFartRects { get; private set; }
        public List<Rectangle> PlayerMerdaRects { get; private set; }

        // In this class there are two different ways to load textures/sprites,
        // because this is an old project that I reworked to make it build with MonoGame
        public IDictionary<string, Sprite> OtherSprites { get; private set; }

        public SpriteFont Font { get; private set; }

        public IDictionary<string, SoundEffect> EnvironmentalSounds { get; private set; }
        public IDictionary<string, SoundEffect> FartsSounds { get; private set; }
        public IDictionary<string, Rectangle> TexturesRectangles { get; private set; }

        public AssetsLoader(ContentManager contentManager)
        {
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            LoadResources();
        }

        private void LoadResources()
        {
            Font = _contentManager.Load<SpriteFont>("TextFont");
            Textures = _contentManager.Load<Texture2D>("textures");
            TexturesBuildingsBack = _contentManager.Load<Texture2D>("buildings_back");
            TexturesBuildingsMid = _contentManager.Load<Texture2D>("buildings_mid");
            TexturesBuildingsGround = _contentManager.Load<Texture2D>("buildings_ground");

            OtherSprites = new Dictionary<string, Sprite>();

            {
                var texture = _contentManager.Load<Texture2D>("manina");
                OtherSprites.Add("manina", LoadSpriteFromTexture(texture));
            }

            {
                var texture = _contentManager.Load<Texture2D>("menuBackground");
                OtherSprites.Add("menuBackground", LoadSpriteFromTexture(texture));
            }

            {
                var texture = _contentManager.Load<Texture2D>("gameoverBackground");
                OtherSprites.Add("gameoverBackground", LoadSpriteFromTexture(texture));
            }

            {
                var texture = _contentManager.Load<Texture2D>("scoreBackground");
                OtherSprites.Add("scoreBackground", LoadSpriteFromTexture(texture));
            }

            {
                var texture = _contentManager.Load<Texture2D>("gameTitle");
                OtherSprites.Add("gameTitle", LoadSpriteFromTexture(texture));
            }

            TexturesRectangles = new Dictionary<string, Rectangle>
            {
                { "Bang", new Rectangle(824, 68, 251, 171) },
                { "BroccoloParticle", new Rectangle(1719, 542, 64, 64) },
                { "Bean", new Rectangle(2000, 105, 34, 48) },
                { "Burger", new Rectangle(1992, 2, 51, 43) },
                { "GameOver", new Rectangle(824, 2, 400, 64) },
                { "JalapenoParticle", new Rectangle(1077, 250, 128, 128) },
                { "Jalapenos", new Rectangle(1988, 301, 46, 61) },
                { "Merda", new Rectangle(1988, 254, 57, 45) },
                { "Pause", new Rectangle(1990, 152, 50, 50) },
                { "Play", new Rectangle(1992, 100, 50, 50) },
                { "Record", new Rectangle(1226, 2, 255, 223) },
                { "ScoreggiaParticle", new Rectangle(1719, 476, 64, 64) },
                { "Stella", new Rectangle(1990, 204, 48, 48) },
                { "Verdura", new Rectangle(1992, 47, 50, 51) },
                { "background", new Rectangle(2, 2, 800, 1500) },
                { "death_screen", new Rectangle(2, 1504, 800, 430) },
                { "fall/fall__000", new Rectangle(1335, 435, 87, 102) },
                { "fall/fall__001", new Rectangle(1076, 584, 87, 100) },
                { "fall/fall__002", new Rectangle(1076, 482, 89, 100) },
                { "fall/fall__003", new Rectangle(1077, 380, 89, 100) },
                { "fall/fall__004", new Rectangle(1374, 331, 89, 102) },
                { "fall/fall__005", new Rectangle(1379, 227, 89, 102) },
                { "fall/fall__006", new Rectangle(1374, 331, 89, 102) },
                { "fall/fall__007", new Rectangle(1077, 380, 89, 100) },
                { "fall/fall__008", new Rectangle(1076, 482, 89, 100) },
                { "fall/fall__009", new Rectangle(1076, 584, 87, 100) },
                { "fart_sustain_up/fart_up__000", new Rectangle(991, 553, 83, 96) },
                { "fart_sustain_up/fart_up__001", new Rectangle(991, 453, 83, 98) },
                { "fart_sustain_up/fart_up__002", new Rectangle(1422, 616, 81, 96) },
                { "fart_sustain_up/fart_up__003", new Rectangle(1588, 674, 81, 92) },
                { "fart_sustain_up/fart_up__004", new Rectangle(1252, 723, 81, 94) },
                { "fart_sustain_up/fart_up__005", new Rectangle(1963, 606, 83, 94) },
                { "fart_sustain_up/fart_up__006", new Rectangle(1600, 576, 85, 96) },
                { "fart_sustain_up/fart_up__007", new Rectangle(1513, 516, 85, 98) },
                { "fart_sustain_up/fart_up__008", new Rectangle(1424, 516, 87, 98) },
                { "fart_sustain_up/fart_up__009", new Rectangle(1874, 516, 87, 98) },
                { "fart_sustain_up/fart_up__010", new Rectangle(1167, 529, 87, 100) },
                { "fart_sustain_up/fart_up__011", new Rectangle(1785, 516, 87, 98) },
                { "fart_sustain_up/fart_up__012", new Rectangle(1882, 416, 87, 98) },
                { "fart_sustain_up/fart_up__013", new Rectangle(1630, 476, 87, 98) },
                { "fart_sustain_up/fart_up__014", new Rectangle(1793, 416, 87, 98) },
                { "fart_sustain_up/fart_up__015", new Rectangle(913, 651, 87, 98) },
                { "fart_sustain_up/fart_up__016", new Rectangle(824, 651, 87, 98) },
                { "fart_sustain_up/fart_up__017", new Rectangle(1075, 686, 85, 98) },
                { "fart_sustain_up/fart_up__018", new Rectangle(1165, 631, 85, 98) },
                { "fart_sustain_up/fart_up__019", new Rectangle(1335, 539, 85, 98) },
                { "idle/idle__000", new Rectangle(1505, 710, 80, 104) },
                { "idle/idle__001", new Rectangle(995, 347, 80, 104) },
                { "idle/idle__002", new Rectangle(1667, 812, 78, 104) },
                { "idle/idle__003", new Rectangle(1751, 712, 78, 104) },
                { "idle/idle__004", new Rectangle(1851, 616, 78, 104) },
                { "idle/idle__005", new Rectangle(995, 241, 80, 104) },
                { "idle/idle__006", new Rectangle(1407, 910, 78, 104) },
                { "idle/idle__007", new Rectangle(1497, 816, 78, 104) },
                { "idle/idle__008", new Rectangle(1587, 768, 78, 104) },
                { "idle/idle__009", new Rectangle(1671, 706, 78, 104) },
                { "idle/idle__010", new Rectangle(742, 1936, 80, 104) },
                { "idle/idle__011", new Rectangle(660, 1936, 80, 104) },
                { "idle/idle__012", new Rectangle(578, 1936, 80, 104) },
                { "idle/idle__013", new Rectangle(496, 1936, 80, 104) },
                { "idle/idle__014", new Rectangle(2, 1936, 82, 104) },
                { "idle/idle__015", new Rectangle(414, 1936, 80, 104) },
                { "idle/idle__016", new Rectangle(332, 1936, 80, 104) },
                { "idle/idle__017", new Rectangle(250, 1936, 80, 104) },
                { "idle/idle__018", new Rectangle(168, 1936, 80, 104) },
                { "idle/idle__019", new Rectangle(86, 1936, 80, 104) },
                { "merdone/merdone__000", new Rectangle(1483, 2, 169, 102) },
                { "merdone/merdone__001", new Rectangle(824, 345, 169, 102) },
                { "merdone/merdone__002", new Rectangle(1652, 208, 167, 100) },
                { "merdone/merdone__003", new Rectangle(1821, 106, 167, 100) },
                { "merdone/merdone__004", new Rectangle(1652, 106, 167, 100) },
                { "merdone/merdone__005", new Rectangle(1168, 429, 165, 98) },
                { "merdone/merdone__006", new Rectangle(824, 551, 165, 98) },
                { "merdone/merdone__007", new Rectangle(1470, 314, 165, 98) },
                { "merdone/merdone__008", new Rectangle(1207, 329, 165, 98) },
                { "merdone/merdone__009", new Rectangle(824, 449, 165, 100) },
                { "merdone/merdone__010", new Rectangle(1465, 414, 163, 100) },
                { "merdone/merdone__011", new Rectangle(1212, 227, 165, 100) },
                { "merdone/merdone__012", new Rectangle(1817, 312, 165, 102) },
                { "merdone/merdone__013", new Rectangle(1650, 310, 165, 102) },
                { "merdone/merdone__014", new Rectangle(1821, 208, 165, 102) },
                { "merdone/merdone__015", new Rectangle(1483, 210, 165, 102) },
                { "merdone/merdone__016", new Rectangle(1483, 106, 167, 102) },
                { "merdone/merdone__017", new Rectangle(1823, 2, 167, 102) },
                { "merdone/merdone__018", new Rectangle(1654, 2, 167, 102) },
                { "merdone/merdone__019", new Rectangle(824, 241, 169, 102) },
                { "nuvola1", new Rectangle(1077, 182, 133, 66) },
                { "nuvola2", new Rectangle(1630, 414, 161, 60) },
                { "nuvola3", new Rectangle(1077, 68, 147, 112) },
                { "run/run__000", new Rectangle(1770, 616, 79, 94) },
                { "run/run__001", new Rectangle(1831, 722, 77, 94) },
                { "run/run__002", new Rectangle(1931, 702, 77, 94) },
                { "run/run__003", new Rectangle(1487, 922, 77, 94) },
                { "run/run__004", new Rectangle(1326, 833, 79, 94) },
                { "run/run__005", new Rectangle(1505, 616, 81, 92) },
                { "run/run__006", new Rectangle(1687, 608, 81, 96) },
                { "run/run__007", new Rectangle(1416, 812, 79, 96) },
                { "run/run__008", new Rectangle(1256, 529, 77, 96) },
                { "run/run__009", new Rectangle(1910, 798, 73, 94) },
                { "run/run__010", new Rectangle(1002, 651, 71, 92) },
                { "run/run__011", new Rectangle(1971, 416, 75, 92) },
                { "run/run__012", new Rectangle(1971, 510, 73, 94) },
                { "run/run__013", new Rectangle(1577, 874, 77, 94) },
                { "run/run__014", new Rectangle(1245, 819, 79, 94) },
                { "run/run__015", new Rectangle(1162, 731, 81, 92) },
                { "run/run__016", new Rectangle(1335, 639, 81, 94) },
                { "run/run__017", new Rectangle(1335, 735, 79, 96) },
                { "run/run__018", new Rectangle(1418, 714, 79, 96) },
                { "run/run__019", new Rectangle(1256, 627, 77, 94) },
                { "back1", new Rectangle(1218, 1460, 72, 200) },
                { "back10", new Rectangle(1176, 1708, 40, 309) },
                { "back11", new Rectangle(1546, 2, 104, 350) },
                { "back12", new Rectangle(1218, 1662, 40, 375) },
                { "back13", new Rectangle(1936, 2, 40, 388) },
                { "back14", new Rectangle(594, 1631, 72, 409) },
                { "back15", new Rectangle(1852, 2, 40, 434) },
                { "back16", new Rectangle(1810, 2, 40, 475) },
                { "back17", new Rectangle(1768, 2, 40, 500) },
                { "back18", new Rectangle(774, 1506, 72, 525) },
                { "back19", new Rectangle(1228, 2, 104, 550) },
                { "back2", new Rectangle(1218, 1258, 72, 200) },
                { "back20", new Rectangle(1546, 354, 72, 575) },
                { "back21", new Rectangle(1134, 604, 40, 600) },
                { "back22", new Rectangle(1080, 2, 40, 609) },
                { "back23", new Rectangle(1060, 654, 72, 650) },
                { "back24", new Rectangle(964, 2, 40, 675) },
                { "back25", new Rectangle(806, 713, 104, 700) },
                { "back26", new Rectangle(1028, 1331, 72, 650) },
                { "back27", new Rectangle(1102, 1306, 72, 600) },
                { "back28", new Rectangle(1176, 1156, 40, 550) },
                { "back29", new Rectangle(1228, 554, 72, 500) },
                { "back3", new Rectangle(1978, 204, 40, 200) },
                { "back30", new Rectangle(1440, 2, 104, 450) },
                { "back31", new Rectangle(1292, 1558, 40, 475) },
                { "back32", new Rectangle(1726, 2, 40, 500) },
                { "back33", new Rectangle(1334, 2, 104, 509) },
                { "back34", new Rectangle(1176, 604, 40, 550) },
                { "back35", new Rectangle(1440, 454, 72, 563) },
                { "back36", new Rectangle(1652, 2, 72, 588) },
                { "back37", new Rectangle(848, 1415, 104, 625) },
                { "back38", new Rectangle(1006, 2, 72, 650) },
                { "back39", new Rectangle(954, 1378, 72, 663) },
                { "back4", new Rectangle(1978, 2, 40, 200) },
                { "back40", new Rectangle(890, 2, 72, 688) },
                { "back41", new Rectangle(784, 2, 104, 709) },
                { "back42", new Rectangle(710, 2, 72, 750) },
                { "back43", new Rectangle(658, 804, 104, 775) },
                { "back44", new Rectangle(636, 2, 72, 800) },
                { "back45", new Rectangle(562, 2, 72, 825) },
                { "back46", new Rectangle(488, 2, 72, 850) },
                { "back47", new Rectangle(404, 888, 72, 875) },
                { "back48", new Rectangle(382, 2, 104, 884) },
                { "back49", new Rectangle(340, 2, 40, 925) },
                { "back5", new Rectangle(1218, 1056, 72, 200) },
                { "back50", new Rectangle(2, 1104, 104, 934) },
                { "back51", new Rectangle(298, 2, 40, 975) },
                { "back52", new Rectangle(108, 1054, 104, 984) },
                { "back53", new Rectangle(192, 2, 104, 1025) },
                { "back54", new Rectangle(76, 2, 72, 1050) },
                { "back55", new Rectangle(2, 2, 72, 1100) },
                { "back56", new Rectangle(150, 2, 40, 1050) },
                { "back57", new Rectangle(214, 1029, 104, 1000) },
                { "back58", new Rectangle(320, 979, 40, 950) },
                { "back59", new Rectangle(362, 929, 40, 888) },
                { "back6", new Rectangle(362, 1819, 40, 225) },
                { "back60", new Rectangle(478, 888, 104, 850) },
                { "back61", new Rectangle(584, 829, 72, 800) },
                { "back62", new Rectangle(764, 754, 40, 750) },
                { "back63", new Rectangle(912, 692, 72, 684) },
                { "back64", new Rectangle(986, 679, 72, 650) },
                { "back65", new Rectangle(1122, 2, 104, 600) },
                { "back66", new Rectangle(1334, 513, 72, 550) },
                { "back67", new Rectangle(1292, 1056, 40, 500) },
                { "back68", new Rectangle(668, 1581, 104, 450) },
                { "back69", new Rectangle(1894, 2, 40, 400) },
                { "back7", new Rectangle(552, 1740, 40, 238) },
                { "back8", new Rectangle(404, 1765, 104, 275) },
                { "back9", new Rectangle(510, 1740, 40, 300) },
                { "ground1", new Rectangle(1274, 1281, 80, 100) },
                { "ground10", new Rectangle(344, 1806, 80, 225) },
                { "ground11", new Rectangle(1844, 354, 144, 250) },
                { "ground12", new Rectangle(1502, 1106, 112, 275) },
                { "ground13", new Rectangle(1680, 1735, 80, 300) },
                { "ground14", new Rectangle(426, 1706, 144, 325) },
                { "ground15", new Rectangle(1926, 1410, 80, 350) },
                { "ground16", new Rectangle(1680, 1358, 80, 375) },
                { "ground17", new Rectangle(1762, 1333, 80, 400) },
                { "ground18", new Rectangle(1762, 906, 80, 425) },
                { "ground19", new Rectangle(1762, 454, 80, 450) },
                { "ground2", new Rectangle(1192, 1281, 80, 100) },
                { "ground20", new Rectangle(1630, 504, 112, 475) },
                { "ground21", new Rectangle(1534, 2, 112, 500) },
                { "ground22", new Rectangle(1434, 1383, 80, 525) },
                { "ground23", new Rectangle(914, 1481, 144, 550) },
                { "ground24", new Rectangle(1288, 604, 112, 575) },
                { "ground25", new Rectangle(1192, 2, 144, 600) },
                { "ground26", new Rectangle(1452, 2, 80, 550) },
                { "ground27", new Rectangle(1598, 1383, 80, 500) },
                { "ground28", new Rectangle(1730, 2, 112, 450) },
                { "ground29", new Rectangle(1958, 1008, 80, 400) },
                { "ground3", new Rectangle(1110, 1331, 80, 100) },
                { "ground30", new Rectangle(1844, 1410, 80, 350) },
                { "ground31", new Rectangle(1844, 1033, 112, 375) },
                { "ground32", new Rectangle(572, 1631, 112, 400) },
                { "ground33", new Rectangle(1844, 606, 112, 425) },
                { "ground34", new Rectangle(686, 1581, 112, 450) },
                { "ground35", new Rectangle(1648, 2, 80, 475) },
                { "ground36", new Rectangle(1516, 1383, 80, 500) },
                { "ground37", new Rectangle(800, 1506, 112, 525) },
                { "ground38", new Rectangle(1402, 554, 112, 550) },
                { "ground39", new Rectangle(1288, 1383, 144, 575) },
                { "ground4", new Rectangle(1110, 1331, 80, 100) },
                { "ground40", new Rectangle(1060, 1433, 144, 600) },
                { "ground41", new Rectangle(1174, 654, 112, 625) },
                { "ground42", new Rectangle(1078, 2, 112, 650) },
                { "ground43", new Rectangle(996, 2, 80, 675) },
                { "ground44", new Rectangle(946, 704, 80, 700) },
                { "ground45", new Rectangle(832, 754, 112, 725) },
                { "ground46", new Rectangle(750, 754, 80, 750) },
                { "ground47", new Rectangle(636, 804, 112, 775) },
                { "ground48", new Rectangle(522, 829, 112, 800) },
                { "ground49", new Rectangle(508, 2, 112, 825) },
                { "ground5", new Rectangle(1028, 1331, 80, 100) },
                { "ground50", new Rectangle(408, 854, 112, 850) },
                { "ground51", new Rectangle(312, 2, 80, 875) },
                { "ground52", new Rectangle(166, 2, 144, 900) },
                { "ground53", new Rectangle(148, 954, 144, 925) },
                { "ground54", new Rectangle(84, 2, 80, 950) },
                { "ground55", new Rectangle(2, 2, 80, 1000) },
                { "ground56", new Rectangle(2, 1004, 144, 950) },
                { "ground57", new Rectangle(294, 904, 112, 900) },
                { "ground58", new Rectangle(394, 2, 112, 850) },
                { "ground59", new Rectangle(622, 2, 80, 800) },
                { "ground6", new Rectangle(262, 1881, 80, 125) },
                { "ground60", new Rectangle(704, 2, 144, 750) },
                { "ground61", new Rectangle(850, 2, 144, 700) },
                { "ground62", new Rectangle(1028, 679, 144, 650) },
                { "ground63", new Rectangle(1206, 1383, 80, 600) },
                { "ground64", new Rectangle(1338, 2, 112, 550) },
                { "ground65", new Rectangle(1516, 554, 112, 500) },
                { "ground66", new Rectangle(1958, 606, 80, 400) },
                { "ground67", new Rectangle(1844, 2, 144, 350) },
                { "ground68", new Rectangle(1616, 1056, 144, 300) },
                { "ground69", new Rectangle(1762, 1735, 80, 250) },
                { "ground7", new Rectangle(148, 1881, 112, 150) },
                { "ground8", new Rectangle(1844, 1762, 80, 175) },
                { "ground9", new Rectangle(1356, 1181, 144, 200) },
                { "mid1", new Rectangle(1080, 1106, 104, 150) },
                { "mid10", new Rectangle(1166, 1760, 104, 275) },
                { "mid11", new Rectangle(436, 1731, 104, 300) },
                { "mid12", new Rectangle(1260, 1433, 104, 325) },
                { "mid13", new Rectangle(1514, 2, 104, 350) },
                { "mid14", new Rectangle(1482, 404, 72, 375) },
                { "mid15", new Rectangle(1408, 881, 104, 400) },
                { "mid16", new Rectangle(1376, 454, 104, 425) },
                { "mid17", new Rectangle(1334, 2, 72, 450) },
                { "mid18", new Rectangle(1292, 2, 40, 475) },
                { "mid19", new Rectangle(1186, 1081, 40, 500) },
                { "mid2", new Rectangle(954, 1883, 104, 150) },
                { "mid20", new Rectangle(1176, 2, 40, 525) },
                { "mid21", new Rectangle(1102, 2, 72, 550) },
                { "mid22", new Rectangle(996, 1281, 104, 575) },
                { "mid23", new Rectangle(954, 1281, 40, 600) },
                { "mid24", new Rectangle(912, 1331, 40, 625) },
                { "mid25", new Rectangle(890, 679, 40, 650) },
                { "mid26", new Rectangle(932, 679, 72, 600) },
                { "mid27", new Rectangle(1080, 554, 72, 550) },
                { "mid28", new Rectangle(1218, 2, 72, 500) },
                { "mid29", new Rectangle(1302, 479, 72, 450) },
                { "mid3", new Rectangle(1356, 1760, 72, 150) },
                { "mid30", new Rectangle(1408, 2, 104, 400) },
                { "mid31", new Rectangle(1302, 931, 104, 425) },
                { "mid32", new Rectangle(1228, 981, 72, 450) },
                { "mid33", new Rectangle(1228, 504, 72, 475) },
                { "mid34", new Rectangle(1144, 1258, 40, 500) },
                { "mid35", new Rectangle(1154, 554, 72, 525) },
                { "mid36", new Rectangle(1028, 2, 72, 550) },
                { "mid37", new Rectangle(1006, 604, 72, 575) },
                { "mid38", new Rectangle(986, 2, 40, 600) },
                { "mid39", new Rectangle(828, 1406, 40, 625) },
                { "mid4", new Rectangle(1186, 1583, 72, 150) },
                { "mid40", new Rectangle(870, 1331, 40, 650) },
                { "mid41", new Rectangle(838, 2, 104, 675) },
                { "mid42", new Rectangle(774, 704, 72, 700) },
                { "mid43", new Rectangle(700, 754, 72, 725) },
                { "mid44", new Rectangle(690, 2, 72, 750) },
                { "mid45", new Rectangle(594, 804, 104, 775) },
                { "mid46", new Rectangle(510, 2, 104, 800) },
                { "mid47", new Rectangle(446, 854, 104, 825) },
                { "mid48", new Rectangle(436, 2, 72, 850) },
                { "mid49", new Rectangle(362, 2, 72, 875) },
                { "mid5", new Rectangle(224, 1881, 104, 150) },
                { "mid50", new Rectangle(256, 2, 104, 900) },
                { "mid51", new Rectangle(224, 954, 104, 925) },
                { "mid52", new Rectangle(118, 1004, 104, 950) },
                { "mid53", new Rectangle(2, 1054, 72, 975) },
                { "mid54", new Rectangle(76, 2, 104, 1000) },
                { "mid55", new Rectangle(2, 2, 72, 1050) },
                { "mid56", new Rectangle(76, 1004, 40, 1000) },
                { "mid57", new Rectangle(182, 2, 72, 950) },
                { "mid58", new Rectangle(330, 904, 40, 900) },
                { "mid59", new Rectangle(372, 879, 72, 850) },
                { "mid6", new Rectangle(1060, 1858, 104, 175) },
                { "mid60", new Rectangle(552, 804, 40, 800) },
                { "mid61", new Rectangle(616, 2, 72, 750) },
                { "mid62", new Rectangle(764, 2, 72, 700) },
                { "mid63", new Rectangle(848, 679, 40, 650) },
                { "mid64", new Rectangle(944, 2, 40, 600) },
                { "mid65", new Rectangle(722, 1481, 104, 550) },
                { "mid66", new Rectangle(1102, 1258, 40, 500) },
                { "mid67", new Rectangle(648, 1581, 72, 450) },
                { "mid68", new Rectangle(1366, 1358, 104, 400) },
                { "mid69", new Rectangle(542, 1681, 104, 350) },
                { "mid7", new Rectangle(1314, 1760, 40, 200) },
                { "mid8", new Rectangle(330, 1806, 104, 225) },
                { "mid9", new Rectangle(1272, 1760, 40, 250) }
            };

            PlayerRunRects = new List<Rectangle>
            {
                TexturesRectangles["run/run__000"],
                TexturesRectangles["run/run__001"],
                TexturesRectangles["run/run__002"],
                TexturesRectangles["run/run__003"],
                TexturesRectangles["run/run__004"],
                TexturesRectangles["run/run__005"],
                TexturesRectangles["run/run__006"],
                TexturesRectangles["run/run__007"],
                TexturesRectangles["run/run__008"],
                TexturesRectangles["run/run__009"],
                TexturesRectangles["run/run__010"],
                TexturesRectangles["run/run__011"],
                TexturesRectangles["run/run__012"],
                TexturesRectangles["run/run__013"],
                TexturesRectangles["run/run__014"],
                TexturesRectangles["run/run__015"],
                TexturesRectangles["run/run__016"],
                TexturesRectangles["run/run__017"],
                TexturesRectangles["run/run__018"],
                TexturesRectangles["run/run__019"]
            };

            PlayerFallRects = new List<Rectangle>
            {
                TexturesRectangles["fall/fall__000"],
                TexturesRectangles["fall/fall__001"],
                TexturesRectangles["fall/fall__002"],
                TexturesRectangles["fall/fall__003"],
                TexturesRectangles["fall/fall__004"],
                TexturesRectangles["fall/fall__005"],
                TexturesRectangles["fall/fall__006"],
                TexturesRectangles["fall/fall__007"],
                TexturesRectangles["fall/fall__008"],
                TexturesRectangles["fall/fall__009"]
            };

            PlayerFartRects = new List<Rectangle>
            {
                TexturesRectangles["fart_sustain_up/fart_up__000"],
                TexturesRectangles["fart_sustain_up/fart_up__001"],
                TexturesRectangles["fart_sustain_up/fart_up__002"],
                TexturesRectangles["fart_sustain_up/fart_up__003"],
                TexturesRectangles["fart_sustain_up/fart_up__004"],
                TexturesRectangles["fart_sustain_up/fart_up__005"],
                TexturesRectangles["fart_sustain_up/fart_up__006"],
                TexturesRectangles["fart_sustain_up/fart_up__007"],
                TexturesRectangles["fart_sustain_up/fart_up__008"],
                TexturesRectangles["fart_sustain_up/fart_up__009"],
                TexturesRectangles["fart_sustain_up/fart_up__010"],
                TexturesRectangles["fart_sustain_up/fart_up__011"],
                TexturesRectangles["fart_sustain_up/fart_up__012"],
                TexturesRectangles["fart_sustain_up/fart_up__013"],
                TexturesRectangles["fart_sustain_up/fart_up__014"],
                TexturesRectangles["fart_sustain_up/fart_up__015"],
                TexturesRectangles["fart_sustain_up/fart_up__016"],
                TexturesRectangles["fart_sustain_up/fart_up__017"],
                TexturesRectangles["fart_sustain_up/fart_up__018"],
                TexturesRectangles["fart_sustain_up/fart_up__019"]
            };

            PlayerIdleRects = new List<Rectangle>
            {
                TexturesRectangles["idle/idle__000"],
                TexturesRectangles["idle/idle__001"],
                TexturesRectangles["idle/idle__002"],
                TexturesRectangles["idle/idle__003"],
                TexturesRectangles["idle/idle__004"],
                TexturesRectangles["idle/idle__005"],
                TexturesRectangles["idle/idle__006"],
                TexturesRectangles["idle/idle__007"],
                TexturesRectangles["idle/idle__008"],
                TexturesRectangles["idle/idle__009"],
                TexturesRectangles["idle/idle__010"],
                TexturesRectangles["idle/idle__011"],
                TexturesRectangles["idle/idle__012"],
                TexturesRectangles["idle/idle__013"],
                TexturesRectangles["idle/idle__014"],
                TexturesRectangles["idle/idle__015"],
                TexturesRectangles["idle/idle__016"],
                TexturesRectangles["idle/idle__017"],
                TexturesRectangles["idle/idle__018"],
                TexturesRectangles["idle/idle__019"]
            };

            PlayerMerdaRects = new List<Rectangle>
            {
                TexturesRectangles["merdone/merdone__000"],
                TexturesRectangles["merdone/merdone__001"],
                TexturesRectangles["merdone/merdone__002"],
                TexturesRectangles["merdone/merdone__003"],
                TexturesRectangles["merdone/merdone__004"],
                TexturesRectangles["merdone/merdone__005"],
                TexturesRectangles["merdone/merdone__006"],
                TexturesRectangles["merdone/merdone__007"],
                TexturesRectangles["merdone/merdone__008"],
                TexturesRectangles["merdone/merdone__009"],
                TexturesRectangles["merdone/merdone__010"],
                TexturesRectangles["merdone/merdone__011"],
                TexturesRectangles["merdone/merdone__012"],
                TexturesRectangles["merdone/merdone__013"],
                TexturesRectangles["merdone/merdone__014"],
                TexturesRectangles["merdone/merdone__015"],
                TexturesRectangles["merdone/merdone__016"],
                TexturesRectangles["merdone/merdone__017"],
                TexturesRectangles["merdone/merdone__018"],
                TexturesRectangles["merdone/merdone__019"]
            };

            string environmentalSoundsFolder = Path.Combine("Music", "Effects");
            EnvironmentalSounds = new Dictionary<string, SoundEffect>
            {
                { "heartbeat", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "heartbeat")) },
                { "explosion", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "explosion")) },
                { "fall", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "fall")) },
                { "jalapeno", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "jalapeno")) },
                { "truck", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "truck")) },
                { "bite", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "bite")) },
                { "Music-Menu", _contentManager.Load<SoundEffect>("Music/Music-Menu") },
                { "Music-Game", _contentManager.Load<SoundEffect>("Music/Music-Game") },
                { "thunder", _contentManager.Load<SoundEffect>(Path.Combine(environmentalSoundsFolder, "thunder"))}
            };

            string fartsFolder = Path.Combine("Music", "Farts");
            FartsSounds = new Dictionary<string, SoundEffect>
            {
                { "fart1", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart1")) },
                { "fart2", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart2")) },
                { "fart3", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart3")) },
                { "fart4", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart4")) },
                { "fart5", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart5")) },
                { "fart6", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart6")) },
                { "fart7", _contentManager.Load<SoundEffect>(Path.Combine(fartsFolder, "fart7")) },
            };
        }

        private Sprite LoadSpriteFromTexture(Texture2D texture)
        {
            return new Sprite(
                new FbonizziMonoGame.Assets.SpriteDescription()
                {
                    X = 0,
                    Y = 0,
                    Width = texture.Width,
                    Height = texture.Height
                },
                texture);
        }
    }
}