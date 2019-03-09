using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Infart.Assets
{
    public class Loader
    {
        protected ContentManager content_;

        private bool music_loaded_ = false;

        private bool texture_loaded_ = false;

        public Texture2D textures_;

        public Texture2D textures_gratta_back_;

        public Texture2D textures_gratta_mid_;

        public Texture2D textures_gratta_ground_;

        public Texture2D px_texture_;

        public List<Rectangle> player_run_rects_;

        public List<Rectangle> player_idle_rects_;

        public List<Rectangle> player_fall_rects_;

        public List<Rectangle> player_fart_rects_;

        public List<Rectangle> player_merda_rects_;

        public Dictionary<string, Rectangle> textures_rectangles_;

        public Dictionary<string, SoundEffect> sound_effects_;

        public SpriteFont font_;

        public Loader(
            ContentManager Content,
            GraphicsDevice Graphics)
        {
            content_ = Content;

            font_ = Content.Load<SpriteFont>(@"TextFont");

            px_texture_ = new Texture2D(Graphics, 1, 1);
            px_texture_.SetData(new Color[] { Color.White });
            LoadTexture();
        }

        private void MusicLoader()
        {
            if (!music_loaded_)
            {
                LoadMusic();
                music_loaded_ = true;
            }
        }

        public void LoadTexture()
        {
            if (texture_loaded_)
                return;
            
            texture_loaded_ = true;

            textures_rectangles_ = new Dictionary<string, Rectangle>();
            textures_ = content_.Load<Texture2D>(@"textures");

            textures_gratta_back_ = content_.Load<Texture2D>(@"gratta_back");
            textures_gratta_mid_ = content_.Load<Texture2D>(@"gratta_mid");
            textures_gratta_ground_ = content_.Load<Texture2D>(@"gratta_ground");

            textures_rectangles_.Add("Bang", new Rectangle(824, 68, 251, 171));
            textures_rectangles_.Add("BroccoloParticle", new Rectangle(1719, 542, 64, 64));
            textures_rectangles_.Add("Burger", new Rectangle(1992, 2, 51, 43));
            textures_rectangles_.Add("GameOver", new Rectangle(824, 2, 400, 64));
            textures_rectangles_.Add("JalapenoParticle", new Rectangle(1077, 250, 128, 128));
            textures_rectangles_.Add("Jalapenos", new Rectangle(1988, 301, 46, 61));
            textures_rectangles_.Add("Merda", new Rectangle(1988, 254, 57, 45));
            textures_rectangles_.Add("Pause", new Rectangle(1990, 152, 50, 50));
            textures_rectangles_.Add("Play", new Rectangle(1992, 100, 50, 50));
            textures_rectangles_.Add("Record", new Rectangle(1226, 2, 255, 223));
            textures_rectangles_.Add("ScoreggiaParticle", new Rectangle(1719, 476, 64, 64));
            textures_rectangles_.Add("Stella", new Rectangle(1990, 204, 48, 48));
            textures_rectangles_.Add("Verdura", new Rectangle(1992, 47, 50, 51));
            textures_rectangles_.Add("background", new Rectangle(2, 2, 800, 1500));
            textures_rectangles_.Add("death_screen", new Rectangle(2, 1504, 800, 430));
            textures_rectangles_.Add("fall/fall__000", new Rectangle(1335, 435, 87, 102));
            textures_rectangles_.Add("fall/fall__001", new Rectangle(1076, 584, 87, 100));
            textures_rectangles_.Add("fall/fall__002", new Rectangle(1076, 482, 89, 100));
            textures_rectangles_.Add("fall/fall__003", new Rectangle(1077, 380, 89, 100));
            textures_rectangles_.Add("fall/fall__004", new Rectangle(1374, 331, 89, 102));
            textures_rectangles_.Add("fall/fall__005", new Rectangle(1379, 227, 89, 102));
            textures_rectangles_.Add("fall/fall__006", new Rectangle(1374, 331, 89, 102));
            textures_rectangles_.Add("fall/fall__007", new Rectangle(1077, 380, 89, 100));
            textures_rectangles_.Add("fall/fall__008", new Rectangle(1076, 482, 89, 100));
            textures_rectangles_.Add("fall/fall__009", new Rectangle(1076, 584, 87, 100));
            textures_rectangles_.Add("fart_sustain_up/fart_up__000", new Rectangle(991, 553, 83, 96));
            textures_rectangles_.Add("fart_sustain_up/fart_up__001", new Rectangle(991, 453, 83, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__002", new Rectangle(1422, 616, 81, 96));
            textures_rectangles_.Add("fart_sustain_up/fart_up__003", new Rectangle(1588, 674, 81, 92));
            textures_rectangles_.Add("fart_sustain_up/fart_up__004", new Rectangle(1252, 723, 81, 94));
            textures_rectangles_.Add("fart_sustain_up/fart_up__005", new Rectangle(1963, 606, 83, 94));
            textures_rectangles_.Add("fart_sustain_up/fart_up__006", new Rectangle(1600, 576, 85, 96));
            textures_rectangles_.Add("fart_sustain_up/fart_up__007", new Rectangle(1513, 516, 85, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__008", new Rectangle(1424, 516, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__009", new Rectangle(1874, 516, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__010", new Rectangle(1167, 529, 87, 100));
            textures_rectangles_.Add("fart_sustain_up/fart_up__011", new Rectangle(1785, 516, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__012", new Rectangle(1882, 416, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__013", new Rectangle(1630, 476, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__014", new Rectangle(1793, 416, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__015", new Rectangle(913, 651, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__016", new Rectangle(824, 651, 87, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__017", new Rectangle(1075, 686, 85, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__018", new Rectangle(1165, 631, 85, 98));
            textures_rectangles_.Add("fart_sustain_up/fart_up__019", new Rectangle(1335, 539, 85, 98));
            textures_rectangles_.Add("idle/idle__000", new Rectangle(1505, 710, 80, 104));
            textures_rectangles_.Add("idle/idle__001", new Rectangle(995, 347, 80, 104));
            textures_rectangles_.Add("idle/idle__002", new Rectangle(1667, 812, 78, 104));
            textures_rectangles_.Add("idle/idle__003", new Rectangle(1751, 712, 78, 104));
            textures_rectangles_.Add("idle/idle__004", new Rectangle(1851, 616, 78, 104));
            textures_rectangles_.Add("idle/idle__005", new Rectangle(995, 241, 80, 104));
            textures_rectangles_.Add("idle/idle__006", new Rectangle(1407, 910, 78, 104));
            textures_rectangles_.Add("idle/idle__007", new Rectangle(1497, 816, 78, 104));
            textures_rectangles_.Add("idle/idle__008", new Rectangle(1587, 768, 78, 104));
            textures_rectangles_.Add("idle/idle__009", new Rectangle(1671, 706, 78, 104));
            textures_rectangles_.Add("idle/idle__010", new Rectangle(742, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__011", new Rectangle(660, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__012", new Rectangle(578, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__013", new Rectangle(496, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__014", new Rectangle(2, 1936, 82, 104));
            textures_rectangles_.Add("idle/idle__015", new Rectangle(414, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__016", new Rectangle(332, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__017", new Rectangle(250, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__018", new Rectangle(168, 1936, 80, 104));
            textures_rectangles_.Add("idle/idle__019", new Rectangle(86, 1936, 80, 104));
            textures_rectangles_.Add("merdone/merdone__000", new Rectangle(1483, 2, 169, 102));
            textures_rectangles_.Add("merdone/merdone__001", new Rectangle(824, 345, 169, 102));
            textures_rectangles_.Add("merdone/merdone__002", new Rectangle(1652, 208, 167, 100));
            textures_rectangles_.Add("merdone/merdone__003", new Rectangle(1821, 106, 167, 100));
            textures_rectangles_.Add("merdone/merdone__004", new Rectangle(1652, 106, 167, 100));
            textures_rectangles_.Add("merdone/merdone__005", new Rectangle(1168, 429, 165, 98));
            textures_rectangles_.Add("merdone/merdone__006", new Rectangle(824, 551, 165, 98));
            textures_rectangles_.Add("merdone/merdone__007", new Rectangle(1470, 314, 165, 98));
            textures_rectangles_.Add("merdone/merdone__008", new Rectangle(1207, 329, 165, 98));
            textures_rectangles_.Add("merdone/merdone__009", new Rectangle(824, 449, 165, 100));
            textures_rectangles_.Add("merdone/merdone__010", new Rectangle(1465, 414, 163, 100));
            textures_rectangles_.Add("merdone/merdone__011", new Rectangle(1212, 227, 165, 100));
            textures_rectangles_.Add("merdone/merdone__012", new Rectangle(1817, 312, 165, 102));
            textures_rectangles_.Add("merdone/merdone__013", new Rectangle(1650, 310, 165, 102));
            textures_rectangles_.Add("merdone/merdone__014", new Rectangle(1821, 208, 165, 102));
            textures_rectangles_.Add("merdone/merdone__015", new Rectangle(1483, 210, 165, 102));
            textures_rectangles_.Add("merdone/merdone__016", new Rectangle(1483, 106, 167, 102));
            textures_rectangles_.Add("merdone/merdone__017", new Rectangle(1823, 2, 167, 102));
            textures_rectangles_.Add("merdone/merdone__018", new Rectangle(1654, 2, 167, 102));
            textures_rectangles_.Add("merdone/merdone__019", new Rectangle(824, 241, 169, 102));
            textures_rectangles_.Add("nuvola1", new Rectangle(1077, 182, 133, 66));
            textures_rectangles_.Add("nuvola2", new Rectangle(1630, 414, 161, 60));
            textures_rectangles_.Add("nuvola3", new Rectangle(1077, 68, 147, 112));
            textures_rectangles_.Add("run/run__000", new Rectangle(1770, 616, 79, 94));
            textures_rectangles_.Add("run/run__001", new Rectangle(1831, 722, 77, 94));
            textures_rectangles_.Add("run/run__002", new Rectangle(1931, 702, 77, 94));
            textures_rectangles_.Add("run/run__003", new Rectangle(1487, 922, 77, 94));
            textures_rectangles_.Add("run/run__004", new Rectangle(1326, 833, 79, 94));
            textures_rectangles_.Add("run/run__005", new Rectangle(1505, 616, 81, 92));
            textures_rectangles_.Add("run/run__006", new Rectangle(1687, 608, 81, 96));
            textures_rectangles_.Add("run/run__007", new Rectangle(1416, 812, 79, 96));
            textures_rectangles_.Add("run/run__008", new Rectangle(1256, 529, 77, 96));
            textures_rectangles_.Add("run/run__009", new Rectangle(1910, 798, 73, 94));
            textures_rectangles_.Add("run/run__010", new Rectangle(1002, 651, 71, 92));
            textures_rectangles_.Add("run/run__011", new Rectangle(1971, 416, 75, 92));
            textures_rectangles_.Add("run/run__012", new Rectangle(1971, 510, 73, 94));
            textures_rectangles_.Add("run/run__013", new Rectangle(1577, 874, 77, 94));
            textures_rectangles_.Add("run/run__014", new Rectangle(1245, 819, 79, 94));
            textures_rectangles_.Add("run/run__015", new Rectangle(1162, 731, 81, 92));
            textures_rectangles_.Add("run/run__016", new Rectangle(1335, 639, 81, 94));
            textures_rectangles_.Add("run/run__017", new Rectangle(1335, 735, 79, 96));
            textures_rectangles_.Add("run/run__018", new Rectangle(1418, 714, 79, 96));
            textures_rectangles_.Add("run/run__019", new Rectangle(1256, 627, 77, 94));

            textures_rectangles_.Add("back1", new Rectangle(1218, 1460, 72, 200));
            textures_rectangles_.Add("back10", new Rectangle(1176, 1708, 40, 309));
            textures_rectangles_.Add("back11", new Rectangle(1546, 2, 104, 350));
            textures_rectangles_.Add("back12", new Rectangle(1218, 1662, 40, 375));
            textures_rectangles_.Add("back13", new Rectangle(1936, 2, 40, 388));
            textures_rectangles_.Add("back14", new Rectangle(594, 1631, 72, 409));
            textures_rectangles_.Add("back15", new Rectangle(1852, 2, 40, 434));
            textures_rectangles_.Add("back16", new Rectangle(1810, 2, 40, 475));
            textures_rectangles_.Add("back17", new Rectangle(1768, 2, 40, 500));
            textures_rectangles_.Add("back18", new Rectangle(774, 1506, 72, 525));
            textures_rectangles_.Add("back19", new Rectangle(1228, 2, 104, 550));
            textures_rectangles_.Add("back2", new Rectangle(1218, 1258, 72, 200));
            textures_rectangles_.Add("back20", new Rectangle(1546, 354, 72, 575));
            textures_rectangles_.Add("back21", new Rectangle(1134, 604, 40, 600));
            textures_rectangles_.Add("back22", new Rectangle(1080, 2, 40, 609));
            textures_rectangles_.Add("back23", new Rectangle(1060, 654, 72, 650));
            textures_rectangles_.Add("back24", new Rectangle(964, 2, 40, 675));
            textures_rectangles_.Add("back25", new Rectangle(806, 713, 104, 700));
            textures_rectangles_.Add("back26", new Rectangle(1028, 1331, 72, 650));
            textures_rectangles_.Add("back27", new Rectangle(1102, 1306, 72, 600));
            textures_rectangles_.Add("back28", new Rectangle(1176, 1156, 40, 550));
            textures_rectangles_.Add("back29", new Rectangle(1228, 554, 72, 500));
            textures_rectangles_.Add("back3", new Rectangle(1978, 204, 40, 200));
            textures_rectangles_.Add("back30", new Rectangle(1440, 2, 104, 450));
            textures_rectangles_.Add("back31", new Rectangle(1292, 1558, 40, 475));
            textures_rectangles_.Add("back32", new Rectangle(1726, 2, 40, 500));
            textures_rectangles_.Add("back33", new Rectangle(1334, 2, 104, 509));
            textures_rectangles_.Add("back34", new Rectangle(1176, 604, 40, 550));
            textures_rectangles_.Add("back35", new Rectangle(1440, 454, 72, 563));
            textures_rectangles_.Add("back36", new Rectangle(1652, 2, 72, 588));
            textures_rectangles_.Add("back37", new Rectangle(848, 1415, 104, 625));
            textures_rectangles_.Add("back38", new Rectangle(1006, 2, 72, 650));
            textures_rectangles_.Add("back39", new Rectangle(954, 1378, 72, 663));
            textures_rectangles_.Add("back4", new Rectangle(1978, 2, 40, 200));
            textures_rectangles_.Add("back40", new Rectangle(890, 2, 72, 688));
            textures_rectangles_.Add("back41", new Rectangle(784, 2, 104, 709));
            textures_rectangles_.Add("back42", new Rectangle(710, 2, 72, 750));
            textures_rectangles_.Add("back43", new Rectangle(658, 804, 104, 775));
            textures_rectangles_.Add("back44", new Rectangle(636, 2, 72, 800));
            textures_rectangles_.Add("back45", new Rectangle(562, 2, 72, 825));
            textures_rectangles_.Add("back46", new Rectangle(488, 2, 72, 850));
            textures_rectangles_.Add("back47", new Rectangle(404, 888, 72, 875));
            textures_rectangles_.Add("back48", new Rectangle(382, 2, 104, 884));
            textures_rectangles_.Add("back49", new Rectangle(340, 2, 40, 925));
            textures_rectangles_.Add("back5", new Rectangle(1218, 1056, 72, 200));
            textures_rectangles_.Add("back50", new Rectangle(2, 1104, 104, 934));
            textures_rectangles_.Add("back51", new Rectangle(298, 2, 40, 975));
            textures_rectangles_.Add("back52", new Rectangle(108, 1054, 104, 984));
            textures_rectangles_.Add("back53", new Rectangle(192, 2, 104, 1025));
            textures_rectangles_.Add("back54", new Rectangle(76, 2, 72, 1050));
            textures_rectangles_.Add("back55", new Rectangle(2, 2, 72, 1100));
            textures_rectangles_.Add("back56", new Rectangle(150, 2, 40, 1050));
            textures_rectangles_.Add("back57", new Rectangle(214, 1029, 104, 1000));
            textures_rectangles_.Add("back58", new Rectangle(320, 979, 40, 950));
            textures_rectangles_.Add("back59", new Rectangle(362, 929, 40, 888));
            textures_rectangles_.Add("back6", new Rectangle(362, 1819, 40, 225));
            textures_rectangles_.Add("back60", new Rectangle(478, 888, 104, 850));
            textures_rectangles_.Add("back61", new Rectangle(584, 829, 72, 800));
            textures_rectangles_.Add("back62", new Rectangle(764, 754, 40, 750));
            textures_rectangles_.Add("back63", new Rectangle(912, 692, 72, 684));
            textures_rectangles_.Add("back64", new Rectangle(986, 679, 72, 650));
            textures_rectangles_.Add("back65", new Rectangle(1122, 2, 104, 600));
            textures_rectangles_.Add("back66", new Rectangle(1334, 513, 72, 550));
            textures_rectangles_.Add("back67", new Rectangle(1292, 1056, 40, 500));
            textures_rectangles_.Add("back68", new Rectangle(668, 1581, 104, 450));
            textures_rectangles_.Add("back69", new Rectangle(1894, 2, 40, 400));
            textures_rectangles_.Add("back7", new Rectangle(552, 1740, 40, 238));
            textures_rectangles_.Add("back8", new Rectangle(404, 1765, 104, 275));
            textures_rectangles_.Add("back9", new Rectangle(510, 1740, 40, 300));

            textures_rectangles_.Add("ground1", new Rectangle(1274, 1281, 80, 100));
            textures_rectangles_.Add("ground10", new Rectangle(344, 1806, 80, 225));
            textures_rectangles_.Add("ground11", new Rectangle(1844, 354, 144, 250));
            textures_rectangles_.Add("ground12", new Rectangle(1502, 1106, 112, 275));
            textures_rectangles_.Add("ground13", new Rectangle(1680, 1735, 80, 300));
            textures_rectangles_.Add("ground14", new Rectangle(426, 1706, 144, 325));
            textures_rectangles_.Add("ground15", new Rectangle(1926, 1410, 80, 350));
            textures_rectangles_.Add("ground16", new Rectangle(1680, 1358, 80, 375));
            textures_rectangles_.Add("ground17", new Rectangle(1762, 1333, 80, 400));
            textures_rectangles_.Add("ground18", new Rectangle(1762, 906, 80, 425));
            textures_rectangles_.Add("ground19", new Rectangle(1762, 454, 80, 450));
            textures_rectangles_.Add("ground2", new Rectangle(1192, 1281, 80, 100));
            textures_rectangles_.Add("ground20", new Rectangle(1630, 504, 112, 475));
            textures_rectangles_.Add("ground21", new Rectangle(1534, 2, 112, 500));
            textures_rectangles_.Add("ground22", new Rectangle(1434, 1383, 80, 525));
            textures_rectangles_.Add("ground23", new Rectangle(914, 1481, 144, 550));
            textures_rectangles_.Add("ground24", new Rectangle(1288, 604, 112, 575));
            textures_rectangles_.Add("ground25", new Rectangle(1192, 2, 144, 600));
            textures_rectangles_.Add("ground26", new Rectangle(1452, 2, 80, 550));
            textures_rectangles_.Add("ground27", new Rectangle(1598, 1383, 80, 500));
            textures_rectangles_.Add("ground28", new Rectangle(1730, 2, 112, 450));
            textures_rectangles_.Add("ground29", new Rectangle(1958, 1008, 80, 400));
            textures_rectangles_.Add("ground3", new Rectangle(1110, 1331, 80, 100));
            textures_rectangles_.Add("ground30", new Rectangle(1844, 1410, 80, 350));
            textures_rectangles_.Add("ground31", new Rectangle(1844, 1033, 112, 375));
            textures_rectangles_.Add("ground32", new Rectangle(572, 1631, 112, 400));
            textures_rectangles_.Add("ground33", new Rectangle(1844, 606, 112, 425));
            textures_rectangles_.Add("ground34", new Rectangle(686, 1581, 112, 450));
            textures_rectangles_.Add("ground35", new Rectangle(1648, 2, 80, 475));
            textures_rectangles_.Add("ground36", new Rectangle(1516, 1383, 80, 500));
            textures_rectangles_.Add("ground37", new Rectangle(800, 1506, 112, 525));
            textures_rectangles_.Add("ground38", new Rectangle(1402, 554, 112, 550));
            textures_rectangles_.Add("ground39", new Rectangle(1288, 1383, 144, 575));
            textures_rectangles_.Add("ground4", new Rectangle(1110, 1331, 80, 100));
            textures_rectangles_.Add("ground40", new Rectangle(1060, 1433, 144, 600));
            textures_rectangles_.Add("ground41", new Rectangle(1174, 654, 112, 625));
            textures_rectangles_.Add("ground42", new Rectangle(1078, 2, 112, 650));
            textures_rectangles_.Add("ground43", new Rectangle(996, 2, 80, 675));
            textures_rectangles_.Add("ground44", new Rectangle(946, 704, 80, 700));
            textures_rectangles_.Add("ground45", new Rectangle(832, 754, 112, 725));
            textures_rectangles_.Add("ground46", new Rectangle(750, 754, 80, 750));
            textures_rectangles_.Add("ground47", new Rectangle(636, 804, 112, 775));
            textures_rectangles_.Add("ground48", new Rectangle(522, 829, 112, 800));
            textures_rectangles_.Add("ground49", new Rectangle(508, 2, 112, 825));
            textures_rectangles_.Add("ground5", new Rectangle(1028, 1331, 80, 100));
            textures_rectangles_.Add("ground50", new Rectangle(408, 854, 112, 850));
            textures_rectangles_.Add("ground51", new Rectangle(312, 2, 80, 875));
            textures_rectangles_.Add("ground52", new Rectangle(166, 2, 144, 900));
            textures_rectangles_.Add("ground53", new Rectangle(148, 954, 144, 925));
            textures_rectangles_.Add("ground54", new Rectangle(84, 2, 80, 950));
            textures_rectangles_.Add("ground55", new Rectangle(2, 2, 80, 1000));
            textures_rectangles_.Add("ground56", new Rectangle(2, 1004, 144, 950));
            textures_rectangles_.Add("ground57", new Rectangle(294, 904, 112, 900));
            textures_rectangles_.Add("ground58", new Rectangle(394, 2, 112, 850));
            textures_rectangles_.Add("ground59", new Rectangle(622, 2, 80, 800));
            textures_rectangles_.Add("ground6", new Rectangle(262, 1881, 80, 125));
            textures_rectangles_.Add("ground60", new Rectangle(704, 2, 144, 750));
            textures_rectangles_.Add("ground61", new Rectangle(850, 2, 144, 700));
            textures_rectangles_.Add("ground62", new Rectangle(1028, 679, 144, 650));
            textures_rectangles_.Add("ground63", new Rectangle(1206, 1383, 80, 600));
            textures_rectangles_.Add("ground64", new Rectangle(1338, 2, 112, 550));
            textures_rectangles_.Add("ground65", new Rectangle(1516, 554, 112, 500));
            textures_rectangles_.Add("ground66", new Rectangle(1958, 606, 80, 400));
            textures_rectangles_.Add("ground67", new Rectangle(1844, 2, 144, 350));
            textures_rectangles_.Add("ground68", new Rectangle(1616, 1056, 144, 300));
            textures_rectangles_.Add("ground69", new Rectangle(1762, 1735, 80, 250));
            textures_rectangles_.Add("ground7", new Rectangle(148, 1881, 112, 150));
            textures_rectangles_.Add("ground8", new Rectangle(1844, 1762, 80, 175));
            textures_rectangles_.Add("ground9", new Rectangle(1356, 1181, 144, 200));

            textures_rectangles_.Add("mid1", new Rectangle(1080, 1106, 104, 150));
            textures_rectangles_.Add("mid10", new Rectangle(1166, 1760, 104, 275));
            textures_rectangles_.Add("mid11", new Rectangle(436, 1731, 104, 300));
            textures_rectangles_.Add("mid12", new Rectangle(1260, 1433, 104, 325));
            textures_rectangles_.Add("mid13", new Rectangle(1514, 2, 104, 350));
            textures_rectangles_.Add("mid14", new Rectangle(1482, 404, 72, 375));
            textures_rectangles_.Add("mid15", new Rectangle(1408, 881, 104, 400));
            textures_rectangles_.Add("mid16", new Rectangle(1376, 454, 104, 425));
            textures_rectangles_.Add("mid17", new Rectangle(1334, 2, 72, 450));
            textures_rectangles_.Add("mid18", new Rectangle(1292, 2, 40, 475));
            textures_rectangles_.Add("mid19", new Rectangle(1186, 1081, 40, 500));
            textures_rectangles_.Add("mid2", new Rectangle(954, 1883, 104, 150));
            textures_rectangles_.Add("mid20", new Rectangle(1176, 2, 40, 525));
            textures_rectangles_.Add("mid21", new Rectangle(1102, 2, 72, 550));
            textures_rectangles_.Add("mid22", new Rectangle(996, 1281, 104, 575));
            textures_rectangles_.Add("mid23", new Rectangle(954, 1281, 40, 600));
            textures_rectangles_.Add("mid24", new Rectangle(912, 1331, 40, 625));
            textures_rectangles_.Add("mid25", new Rectangle(890, 679, 40, 650));
            textures_rectangles_.Add("mid26", new Rectangle(932, 679, 72, 600));
            textures_rectangles_.Add("mid27", new Rectangle(1080, 554, 72, 550));
            textures_rectangles_.Add("mid28", new Rectangle(1218, 2, 72, 500));
            textures_rectangles_.Add("mid29", new Rectangle(1302, 479, 72, 450));
            textures_rectangles_.Add("mid3", new Rectangle(1356, 1760, 72, 150));
            textures_rectangles_.Add("mid30", new Rectangle(1408, 2, 104, 400));
            textures_rectangles_.Add("mid31", new Rectangle(1302, 931, 104, 425));
            textures_rectangles_.Add("mid32", new Rectangle(1228, 981, 72, 450));
            textures_rectangles_.Add("mid33", new Rectangle(1228, 504, 72, 475));
            textures_rectangles_.Add("mid34", new Rectangle(1144, 1258, 40, 500));
            textures_rectangles_.Add("mid35", new Rectangle(1154, 554, 72, 525));
            textures_rectangles_.Add("mid36", new Rectangle(1028, 2, 72, 550));
            textures_rectangles_.Add("mid37", new Rectangle(1006, 604, 72, 575));
            textures_rectangles_.Add("mid38", new Rectangle(986, 2, 40, 600));
            textures_rectangles_.Add("mid39", new Rectangle(828, 1406, 40, 625));
            textures_rectangles_.Add("mid4", new Rectangle(1186, 1583, 72, 150));
            textures_rectangles_.Add("mid40", new Rectangle(870, 1331, 40, 650));
            textures_rectangles_.Add("mid41", new Rectangle(838, 2, 104, 675));
            textures_rectangles_.Add("mid42", new Rectangle(774, 704, 72, 700));
            textures_rectangles_.Add("mid43", new Rectangle(700, 754, 72, 725));
            textures_rectangles_.Add("mid44", new Rectangle(690, 2, 72, 750));
            textures_rectangles_.Add("mid45", new Rectangle(594, 804, 104, 775));
            textures_rectangles_.Add("mid46", new Rectangle(510, 2, 104, 800));
            textures_rectangles_.Add("mid47", new Rectangle(446, 854, 104, 825));
            textures_rectangles_.Add("mid48", new Rectangle(436, 2, 72, 850));
            textures_rectangles_.Add("mid49", new Rectangle(362, 2, 72, 875));
            textures_rectangles_.Add("mid5", new Rectangle(224, 1881, 104, 150));
            textures_rectangles_.Add("mid50", new Rectangle(256, 2, 104, 900));
            textures_rectangles_.Add("mid51", new Rectangle(224, 954, 104, 925));
            textures_rectangles_.Add("mid52", new Rectangle(118, 1004, 104, 950));
            textures_rectangles_.Add("mid53", new Rectangle(2, 1054, 72, 975));
            textures_rectangles_.Add("mid54", new Rectangle(76, 2, 104, 1000));
            textures_rectangles_.Add("mid55", new Rectangle(2, 2, 72, 1050));
            textures_rectangles_.Add("mid56", new Rectangle(76, 1004, 40, 1000));
            textures_rectangles_.Add("mid57", new Rectangle(182, 2, 72, 950));
            textures_rectangles_.Add("mid58", new Rectangle(330, 904, 40, 900));
            textures_rectangles_.Add("mid59", new Rectangle(372, 879, 72, 850));
            textures_rectangles_.Add("mid6", new Rectangle(1060, 1858, 104, 175));
            textures_rectangles_.Add("mid60", new Rectangle(552, 804, 40, 800));
            textures_rectangles_.Add("mid61", new Rectangle(616, 2, 72, 750));
            textures_rectangles_.Add("mid62", new Rectangle(764, 2, 72, 700));
            textures_rectangles_.Add("mid63", new Rectangle(848, 679, 40, 650));
            textures_rectangles_.Add("mid64", new Rectangle(944, 2, 40, 600));
            textures_rectangles_.Add("mid65", new Rectangle(722, 1481, 104, 550));
            textures_rectangles_.Add("mid66", new Rectangle(1102, 1258, 40, 500));
            textures_rectangles_.Add("mid67", new Rectangle(648, 1581, 72, 450));
            textures_rectangles_.Add("mid68", new Rectangle(1366, 1358, 104, 400));
            textures_rectangles_.Add("mid69", new Rectangle(542, 1681, 104, 350));
            textures_rectangles_.Add("mid7", new Rectangle(1314, 1760, 40, 200));
            textures_rectangles_.Add("mid8", new Rectangle(330, 1806, 104, 225));
            textures_rectangles_.Add("mid9", new Rectangle(1272, 1760, 40, 250));

            player_run_rects_ = new List<Rectangle>();
            player_run_rects_.Add(textures_rectangles_["run/run__000"]);
            player_run_rects_.Add(textures_rectangles_["run/run__001"]);
            player_run_rects_.Add(textures_rectangles_["run/run__002"]);
            player_run_rects_.Add(textures_rectangles_["run/run__003"]);
            player_run_rects_.Add(textures_rectangles_["run/run__004"]);
            player_run_rects_.Add(textures_rectangles_["run/run__005"]);
            player_run_rects_.Add(textures_rectangles_["run/run__006"]);
            player_run_rects_.Add(textures_rectangles_["run/run__007"]);
            player_run_rects_.Add(textures_rectangles_["run/run__008"]);
            player_run_rects_.Add(textures_rectangles_["run/run__009"]);
            player_run_rects_.Add(textures_rectangles_["run/run__010"]);
            player_run_rects_.Add(textures_rectangles_["run/run__011"]);
            player_run_rects_.Add(textures_rectangles_["run/run__012"]);
            player_run_rects_.Add(textures_rectangles_["run/run__013"]);
            player_run_rects_.Add(textures_rectangles_["run/run__014"]);
            player_run_rects_.Add(textures_rectangles_["run/run__015"]);
            player_run_rects_.Add(textures_rectangles_["run/run__016"]);
            player_run_rects_.Add(textures_rectangles_["run/run__017"]);
            player_run_rects_.Add(textures_rectangles_["run/run__018"]);
            player_run_rects_.Add(textures_rectangles_["run/run__019"]);

            player_fall_rects_ = new List<Rectangle>();
            player_fall_rects_.Add(textures_rectangles_["fall/fall__000"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__001"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__002"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__003"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__004"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__005"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__006"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__007"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__008"]);
            player_fall_rects_.Add(textures_rectangles_["fall/fall__009"]);

            player_fart_rects_ = new List<Rectangle>();
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__000"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__001"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__002"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__003"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__004"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__005"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__006"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__007"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__008"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__009"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__010"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__011"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__012"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__013"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__014"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__015"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__016"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__017"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__018"]);
            player_fart_rects_.Add(textures_rectangles_["fart_sustain_up/fart_up__019"]);

            player_idle_rects_ = new List<Rectangle>();
            player_idle_rects_.Add(textures_rectangles_["idle/idle__000"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__001"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__002"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__003"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__004"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__005"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__006"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__007"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__008"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__009"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__010"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__011"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__012"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__013"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__014"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__015"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__016"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__017"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__018"]);
            player_idle_rects_.Add(textures_rectangles_["idle/idle__019"]);

            player_merda_rects_ = new List<Rectangle>();
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__000"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__001"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__002"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__003"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__004"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__005"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__006"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__007"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__008"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__009"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__010"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__011"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__012"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__013"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__014"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__015"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__016"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__017"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__018"]);
            player_merda_rects_.Add(textures_rectangles_["merdone/merdone__019"]);
        }

        public void LoadMusic()
        {
            string folder = Path.Combine("Music", "Effects");
            sound_effects_ = new Dictionary<string, SoundEffect>();
            sound_effects_.Add("culata", content_.Load<SoundEffect>(Path.Combine(folder, "culata")));
            sound_effects_.Add("cuore", content_.Load<SoundEffect>(Path.Combine(folder, "cuore")));
            sound_effects_.Add("esplosione", content_.Load<SoundEffect>(Path.Combine(folder, "esplosione")));
            sound_effects_.Add("fall", content_.Load<SoundEffect>(Path.Combine(folder, "fall")));
            sound_effects_.Add("jalapeno", content_.Load<SoundEffect>(Path.Combine(folder, "jalapeno")));
            sound_effects_.Add("merdone", content_.Load<SoundEffect>(Path.Combine(folder, "merdone")));
            sound_effects_.Add("morso", content_.Load<SoundEffect>(Path.Combine(folder, "morso")));
            sound_effects_.Add("night", content_.Load<SoundEffect>(Path.Combine(folder, "night")));
            sound_effects_.Add("turu", content_.Load<SoundEffect>(Path.Combine(folder, "turu")));
        }
    }
}