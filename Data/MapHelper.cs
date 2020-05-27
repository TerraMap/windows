using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TerraMap.Data
{
  // copied from Terraria.MapHelper
  public static class MapHelper
  {
    static Color[][] tileColors = new Color[623][];
    static readonly Color[][] wallColors = new Color[316][];

    public static int[] tileOptionCounts;
    public static int[] wallOptionCounts;
    public static ushort[] tileLookup;
    public static ushort[] wallLookup;
    private static ushort tilePosition;
    private static ushort wallPosition;
    private static ushort liquidPosition;
    private static ushort skyPosition;
    private static ushort dirtPosition;
    private static ushort rockPosition;
    private static ushort hellPosition;
    private static Color[] colorLookup;
    private static ushort[] snowTypes;
    private static ushort wallRangeStart;
    private static ushort wallRangeEnd;

    public static void Initialize()
    {
      for (int i = 0; i < tileColors.Length; i++)
      {
        tileColors[i] = new Color[12];
      }
      tileColors[621][0] = Color.FromRgb(250, 250, 250);
      tileColors[622][0] = Color.FromRgb(235, 235, 249);
      tileColors[518][0] = Color.FromRgb(26, 196, 84);
      tileColors[518][1] = Color.FromRgb(48, 208, 234);
      tileColors[518][2] = Color.FromRgb(135, 196, 26);
      tileColors[519][0] = Color.FromRgb(28, 216, 109);
      tileColors[519][1] = Color.FromRgb(107, 182, 0);
      tileColors[519][2] = Color.FromRgb(75, 184, 230);
      tileColors[519][3] = Color.FromRgb(208, 80, 80);
      tileColors[519][4] = Color.FromRgb(141, 137, 223);
      tileColors[519][5] = Color.FromRgb(182, 175, 130);
      tileColors[549][0] = Color.FromRgb(54, 83, 20);
      tileColors[528][0] = Color.FromRgb(182, 175, 130);
      tileColors[529][0] = Color.FromRgb(99, 150, 8);
      tileColors[529][1] = Color.FromRgb(139, 154, 64);
      tileColors[529][2] = Color.FromRgb(34, 129, 168);
      tileColors[529][3] = Color.FromRgb(180, 82, 82);
      tileColors[529][4] = Color.FromRgb(113, 108, 205);
      Color color = Color.FromRgb(151, 107, 75);
      tileColors[0][0] = color;
      tileColors[5][0] = color;
      tileColors[5][1] = Color.FromRgb(182, 175, 130);
      Color color2 = Color.FromRgb(127, 127, 127);
      tileColors[583][0] = color2;
      tileColors[584][0] = color2;
      tileColors[585][0] = color2;
      tileColors[586][0] = color2;
      tileColors[587][0] = color2;
      tileColors[588][0] = color2;
      tileColors[589][0] = color2;
      tileColors[590][0] = color2;
      tileColors[595][0] = color;
      tileColors[596][0] = color;
      tileColors[615][0] = color;
      tileColors[616][0] = color;
      tileColors[30][0] = color;
      tileColors[191][0] = color;
      tileColors[272][0] = Color.FromRgb(121, 119, 101);
      color = Color.FromRgb(128, 128, 128);
      tileColors[1][0] = color;
      tileColors[38][0] = color;
      tileColors[48][0] = color;
      tileColors[130][0] = color;
      tileColors[138][0] = color;
      tileColors[273][0] = color;
      tileColors[283][0] = color;
      tileColors[618][0] = color;
      tileColors[2][0] = Color.FromRgb(28, 216, 94);
      tileColors[477][0] = Color.FromRgb(28, 216, 94);
      tileColors[492][0] = Color.FromRgb(78, 193, 227);
      color = Color.FromRgb(26, 196, 84);
      tileColors[3][0] = color;
      tileColors[192][0] = color;
      tileColors[73][0] = Color.FromRgb(27, 197, 109);
      tileColors[52][0] = Color.FromRgb(23, 177, 76);
      tileColors[353][0] = Color.FromRgb(28, 216, 94);
      tileColors[20][0] = Color.FromRgb(163, 116, 81);
      tileColors[6][0] = Color.FromRgb(140, 101, 80);
      color = Color.FromRgb(150, 67, 22);
      tileColors[7][0] = color;
      tileColors[47][0] = color;
      tileColors[284][0] = color;
      tileColors[560][0] = color;
      color = Color.FromRgb(185, 164, 23);
      tileColors[8][0] = color;
      tileColors[45][0] = color;
      tileColors[560][2] = color;
      color = Color.FromRgb(185, 194, 195);
      tileColors[9][0] = color;
      tileColors[46][0] = color;
      tileColors[560][1] = color;
      color = Color.FromRgb(98, 95, 167);
      tileColors[22][0] = color;
      tileColors[140][0] = color;
      tileColors[23][0] = Color.FromRgb(141, 137, 223);
      tileColors[24][0] = Color.FromRgb(122, 116, 218);
      tileColors[25][0] = Color.FromRgb(109, 90, 128);
      tileColors[37][0] = Color.FromRgb(104, 86, 84);
      tileColors[39][0] = Color.FromRgb(181, 62, 59);
      tileColors[40][0] = Color.FromRgb(146, 81, 68);
      tileColors[41][0] = Color.FromRgb(66, 84, 109);
      tileColors[481][0] = Color.FromRgb(66, 84, 109);
      tileColors[43][0] = Color.FromRgb(84, 100, 63);
      tileColors[482][0] = Color.FromRgb(84, 100, 63);
      tileColors[44][0] = Color.FromRgb(107, 68, 99);
      tileColors[483][0] = Color.FromRgb(107, 68, 99);
      tileColors[53][0] = Color.FromRgb(186, 168, 84);
      color = Color.FromRgb(190, 171, 94);
      tileColors[151][0] = color;
      tileColors[154][0] = color;
      tileColors[274][0] = color;
      tileColors[328][0] = Color.FromRgb(200, 246, 254);
      tileColors[329][0] = Color.FromRgb(15, 15, 15);
      tileColors[54][0] = Color.FromRgb(200, 246, 254);
      tileColors[56][0] = Color.FromRgb(43, 40, 84);
      tileColors[75][0] = Color.FromRgb(26, 26, 26);
      tileColors[57][0] = Color.FromRgb(68, 68, 76);
      color = Color.FromRgb(142, 66, 66);
      tileColors[58][0] = color;
      tileColors[76][0] = color;
      color = Color.FromRgb(92, 68, 73);
      tileColors[59][0] = color;
      tileColors[120][0] = color;
      tileColors[60][0] = Color.FromRgb(143, 215, 29);
      tileColors[61][0] = Color.FromRgb(135, 196, 26);
      tileColors[74][0] = Color.FromRgb(96, 197, 27);
      tileColors[62][0] = Color.FromRgb(121, 176, 24);
      tileColors[233][0] = Color.FromRgb(107, 182, 29);
      tileColors[63][0] = Color.FromRgb(110, 140, 182);
      tileColors[64][0] = Color.FromRgb(196, 96, 114);
      tileColors[65][0] = Color.FromRgb(56, 150, 97);
      tileColors[66][0] = Color.FromRgb(160, 118, 58);
      tileColors[67][0] = Color.FromRgb(140, 58, 166);
      tileColors[68][0] = Color.FromRgb(125, 191, 197);
      tileColors[566][0] = Color.FromRgb(233, 180, 90);
      tileColors[70][0] = Color.FromRgb(93, 127, 255);
      color = Color.FromRgb(182, 175, 130);
      tileColors[71][0] = color;
      tileColors[72][0] = color;
      tileColors[190][0] = color;
      tileColors[578][0] = Color.FromRgb(172, 155, 110);
      color = Color.FromRgb(73, 120, 17);
      tileColors[80][0] = color;
      tileColors[484][0] = color;
      tileColors[188][0] = color;
      tileColors[80][1] = Color.FromRgb(87, 84, 151);
      tileColors[80][2] = Color.FromRgb(34, 129, 168);
      tileColors[80][3] = Color.FromRgb(130, 56, 55);
      color = Color.FromRgb(11, 80, 143);
      tileColors[107][0] = color;
      tileColors[121][0] = color;
      color = Color.FromRgb(91, 169, 169);
      tileColors[108][0] = color;
      tileColors[122][0] = color;
      color = Color.FromRgb(128, 26, 52);
      tileColors[111][0] = color;
      tileColors[150][0] = color;
      tileColors[109][0] = Color.FromRgb(78, 193, 227);
      tileColors[110][0] = Color.FromRgb(48, 186, 135);
      tileColors[113][0] = Color.FromRgb(48, 208, 234);
      tileColors[115][0] = Color.FromRgb(33, 171, 207);
      tileColors[112][0] = Color.FromRgb(103, 98, 122);
      color = Color.FromRgb(238, 225, 218);
      tileColors[116][0] = color;
      tileColors[118][0] = color;
      tileColors[117][0] = Color.FromRgb(181, 172, 190);
      tileColors[119][0] = Color.FromRgb(107, 92, 108);
      tileColors[123][0] = Color.FromRgb(106, 107, 118);
      tileColors[124][0] = Color.FromRgb(73, 51, 36);
      tileColors[131][0] = Color.FromRgb(52, 52, 52);
      tileColors[145][0] = Color.FromRgb(192, 30, 30);
      tileColors[146][0] = Color.FromRgb(43, 192, 30);
      color = Color.FromRgb(211, 236, 241);
      tileColors[147][0] = color;
      tileColors[148][0] = color;
      tileColors[152][0] = Color.FromRgb(128, 133, 184);
      tileColors[153][0] = Color.FromRgb(239, 141, 126);
      tileColors[155][0] = Color.FromRgb(131, 162, 161);
      tileColors[156][0] = Color.FromRgb(170, 171, 157);
      tileColors[157][0] = Color.FromRgb(104, 100, 126);
      color = Color.FromRgb(145, 81, 85);
      tileColors[158][0] = color;
      tileColors[232][0] = color;
      tileColors[575][0] = Color.FromRgb(125, 61, 65);
      tileColors[159][0] = Color.FromRgb(148, 133, 98);
      tileColors[160][0] = Color.FromRgb(200, 0, 0);
      tileColors[160][1] = Color.FromRgb(0, 200, 0);
      tileColors[160][2] = Color.FromRgb(0, 0, 200);
      tileColors[161][0] = Color.FromRgb(144, 195, 232);
      tileColors[162][0] = Color.FromRgb(184, 219, 240);
      tileColors[163][0] = Color.FromRgb(174, 145, 214);
      tileColors[164][0] = Color.FromRgb(218, 182, 204);
      tileColors[170][0] = Color.FromRgb(27, 109, 69);
      tileColors[171][0] = Color.FromRgb(33, 135, 85);
      color = Color.FromRgb(129, 125, 93);
      tileColors[166][0] = color;
      tileColors[175][0] = color;
      tileColors[167][0] = Color.FromRgb(62, 82, 114);
      color = Color.FromRgb(132, 157, 127);
      tileColors[168][0] = color;
      tileColors[176][0] = color;
      color = Color.FromRgb(152, 171, 198);
      tileColors[169][0] = color;
      tileColors[177][0] = color;
      tileColors[179][0] = Color.FromRgb(49, 134, 114);
      tileColors[180][0] = Color.FromRgb(126, 134, 49);
      tileColors[181][0] = Color.FromRgb(134, 59, 49);
      tileColors[182][0] = Color.FromRgb(43, 86, 140);
      tileColors[183][0] = Color.FromRgb(121, 49, 134);
      tileColors[381][0] = Color.FromRgb(254, 121, 2);
      tileColors[534][0] = Color.FromRgb(114, 254, 2);
      tileColors[536][0] = Color.FromRgb(0, 197, 208);
      tileColors[539][0] = Color.FromRgb(208, 0, 126);
      tileColors[512][0] = Color.FromRgb(49, 134, 114);
      tileColors[513][0] = Color.FromRgb(126, 134, 49);
      tileColors[514][0] = Color.FromRgb(134, 59, 49);
      tileColors[515][0] = Color.FromRgb(43, 86, 140);
      tileColors[516][0] = Color.FromRgb(121, 49, 134);
      tileColors[517][0] = Color.FromRgb(254, 121, 2);
      tileColors[535][0] = Color.FromRgb(114, 254, 2);
      tileColors[537][0] = Color.FromRgb(0, 197, 208);
      tileColors[540][0] = Color.FromRgb(208, 0, 126);
      tileColors[184][0] = Color.FromRgb(29, 106, 88);
      tileColors[184][1] = Color.FromRgb(94, 100, 36);
      tileColors[184][2] = Color.FromRgb(96, 44, 40);
      tileColors[184][3] = Color.FromRgb(34, 63, 102);
      tileColors[184][4] = Color.FromRgb(79, 35, 95);
      tileColors[184][5] = Color.FromRgb(253, 62, 3);
      tileColors[184][6] = Color.FromRgb(22, 123, 62);
      tileColors[184][7] = Color.FromRgb(0, 106, 148);
      tileColors[184][8] = Color.FromRgb(148, 0, 132);
      tileColors[189][0] = Color.FromRgb(223, 255, 255);
      tileColors[193][0] = Color.FromRgb(56, 121, 255);
      tileColors[194][0] = Color.FromRgb(157, 157, 107);
      tileColors[195][0] = Color.FromRgb(134, 22, 34);
      tileColors[196][0] = Color.FromRgb(147, 144, 178);
      tileColors[197][0] = Color.FromRgb(97, 200, 225);
      tileColors[198][0] = Color.FromRgb(62, 61, 52);
      tileColors[199][0] = Color.FromRgb(208, 80, 80);
      tileColors[201][0] = Color.FromRgb(203, 61, 64);
      tileColors[205][0] = Color.FromRgb(186, 50, 52);
      tileColors[200][0] = Color.FromRgb(216, 152, 144);
      tileColors[202][0] = Color.FromRgb(213, 178, 28);
      tileColors[203][0] = Color.FromRgb(128, 44, 45);
      tileColors[204][0] = Color.FromRgb(125, 55, 65);
      tileColors[206][0] = Color.FromRgb(124, 175, 201);
      tileColors[208][0] = Color.FromRgb(88, 105, 118);
      tileColors[211][0] = Color.FromRgb(191, 233, 115);
      tileColors[213][0] = Color.FromRgb(137, 120, 67);
      tileColors[214][0] = Color.FromRgb(103, 103, 103);
      tileColors[221][0] = Color.FromRgb(239, 90, 50);
      tileColors[222][0] = Color.FromRgb(231, 96, 228);
      tileColors[223][0] = Color.FromRgb(57, 85, 101);
      tileColors[224][0] = Color.FromRgb(107, 132, 139);
      tileColors[225][0] = Color.FromRgb(227, 125, 22);
      tileColors[226][0] = Color.FromRgb(141, 56, 0);
      tileColors[229][0] = Color.FromRgb(255, 156, 12);
      tileColors[230][0] = Color.FromRgb(131, 79, 13);
      tileColors[234][0] = Color.FromRgb(53, 44, 41);
      tileColors[235][0] = Color.FromRgb(214, 184, 46);
      tileColors[236][0] = Color.FromRgb(149, 232, 87);
      tileColors[237][0] = Color.FromRgb(255, 241, 51);
      tileColors[238][0] = Color.FromRgb(225, 128, 206);
      tileColors[243][0] = Color.FromRgb(198, 196, 170);
      tileColors[248][0] = Color.FromRgb(219, 71, 38);
      tileColors[249][0] = Color.FromRgb(235, 38, 231);
      tileColors[250][0] = Color.FromRgb(86, 85, 92);
      tileColors[251][0] = Color.FromRgb(235, 150, 23);
      tileColors[252][0] = Color.FromRgb(153, 131, 44);
      tileColors[253][0] = Color.FromRgb(57, 48, 97);
      tileColors[254][0] = Color.FromRgb(248, 158, 92);
      tileColors[255][0] = Color.FromRgb(107, 49, 154);
      tileColors[256][0] = Color.FromRgb(154, 148, 49);
      tileColors[257][0] = Color.FromRgb(49, 49, 154);
      tileColors[258][0] = Color.FromRgb(49, 154, 68);
      tileColors[259][0] = Color.FromRgb(154, 49, 77);
      tileColors[260][0] = Color.FromRgb(85, 89, 118);
      tileColors[261][0] = Color.FromRgb(154, 83, 49);
      tileColors[262][0] = Color.FromRgb(221, 79, 255);
      tileColors[263][0] = Color.FromRgb(250, 255, 79);
      tileColors[264][0] = Color.FromRgb(79, 102, 255);
      tileColors[265][0] = Color.FromRgb(79, 255, 89);
      tileColors[266][0] = Color.FromRgb(255, 79, 79);
      tileColors[267][0] = Color.FromRgb(240, 240, 247);
      tileColors[268][0] = Color.FromRgb(255, 145, 79);
      tileColors[287][0] = Color.FromRgb(79, 128, 17);
      color = Color.FromRgb(122, 217, 232);
      tileColors[275][0] = color;
      tileColors[276][0] = color;
      tileColors[277][0] = color;
      tileColors[278][0] = color;
      tileColors[279][0] = color;
      tileColors[280][0] = color;
      tileColors[281][0] = color;
      tileColors[282][0] = color;
      tileColors[285][0] = color;
      tileColors[286][0] = color;
      tileColors[288][0] = color;
      tileColors[289][0] = color;
      tileColors[290][0] = color;
      tileColors[291][0] = color;
      tileColors[292][0] = color;
      tileColors[293][0] = color;
      tileColors[294][0] = color;
      tileColors[295][0] = color;
      tileColors[296][0] = color;
      tileColors[297][0] = color;
      tileColors[298][0] = color;
      tileColors[299][0] = color;
      tileColors[309][0] = color;
      tileColors[310][0] = color;
      tileColors[413][0] = color;
      tileColors[339][0] = color;
      tileColors[542][0] = color;
      tileColors[358][0] = color;
      tileColors[359][0] = color;
      tileColors[360][0] = color;
      tileColors[361][0] = color;
      tileColors[362][0] = color;
      tileColors[363][0] = color;
      tileColors[364][0] = color;
      tileColors[391][0] = color;
      tileColors[392][0] = color;
      tileColors[393][0] = color;
      tileColors[394][0] = color;
      tileColors[414][0] = color;
      tileColors[505][0] = color;
      tileColors[543][0] = color;
      tileColors[598][0] = color;
      tileColors[521][0] = color;
      tileColors[522][0] = color;
      tileColors[523][0] = color;
      tileColors[524][0] = color;
      tileColors[525][0] = color;
      tileColors[526][0] = color;
      tileColors[527][0] = color;
      tileColors[532][0] = color;
      tileColors[533][0] = color;
      tileColors[538][0] = color;
      tileColors[544][0] = color;
      tileColors[550][0] = color;
      tileColors[551][0] = color;
      tileColors[553][0] = color;
      tileColors[554][0] = color;
      tileColors[555][0] = color;
      tileColors[556][0] = color;
      tileColors[558][0] = color;
      tileColors[559][0] = color;
      tileColors[580][0] = color;
      tileColors[582][0] = color;
      tileColors[599][0] = color;
      tileColors[600][0] = color;
      tileColors[601][0] = color;
      tileColors[602][0] = color;
      tileColors[603][0] = color;
      tileColors[604][0] = color;
      tileColors[605][0] = color;
      tileColors[606][0] = color;
      tileColors[607][0] = color;
      tileColors[608][0] = color;
      tileColors[609][0] = color;
      tileColors[610][0] = color;
      tileColors[611][0] = color;
      tileColors[612][0] = color;
      tileColors[619][0] = color;
      tileColors[620][0] = color;
      tileColors[552][0] = tileColors[53][0];
      tileColors[564][0] = Color.FromRgb(87, 127, 220);
      tileColors[408][0] = Color.FromRgb(85, 83, 82);
      tileColors[409][0] = Color.FromRgb(85, 83, 82);
      tileColors[415][0] = Color.FromRgb(249, 75, 7);
      tileColors[416][0] = Color.FromRgb(0, 160, 170);
      tileColors[417][0] = Color.FromRgb(160, 87, 234);
      tileColors[418][0] = Color.FromRgb(22, 173, 254);
      tileColors[489][0] = Color.FromRgb(255, 29, 136);
      tileColors[490][0] = Color.FromRgb(211, 211, 211);
      tileColors[311][0] = Color.FromRgb(117, 61, 25);
      tileColors[312][0] = Color.FromRgb(204, 93, 73);
      tileColors[313][0] = Color.FromRgb(87, 150, 154);
      tileColors[4][0] = Color.FromRgb(253, 221, 3);
      tileColors[4][1] = Color.FromRgb(253, 221, 3);
      color = Color.FromRgb(253, 221, 3);
      tileColors[93][0] = color;
      tileColors[33][0] = color;
      tileColors[174][0] = color;
      tileColors[100][0] = color;
      tileColors[98][0] = color;
      tileColors[173][0] = color;
      color = Color.FromRgb(119, 105, 79);
      tileColors[11][0] = color;
      tileColors[10][0] = color;
      tileColors[593][0] = color;
      tileColors[594][0] = color;
      color = Color.FromRgb(191, 142, 111);
      tileColors[14][0] = color;
      tileColors[469][0] = color;
      tileColors[486][0] = color;
      tileColors[488][0] = Color.FromRgb(127, 92, 69);
      tileColors[487][0] = color;
      tileColors[487][1] = color;
      tileColors[15][0] = color;
      tileColors[15][1] = color;
      tileColors[497][0] = color;
      tileColors[18][0] = color;
      tileColors[19][0] = color;
      tileColors[55][0] = color;
      tileColors[79][0] = color;
      tileColors[86][0] = color;
      tileColors[87][0] = color;
      tileColors[88][0] = color;
      tileColors[89][0] = color;
      tileColors[94][0] = color;
      tileColors[101][0] = color;
      tileColors[104][0] = color;
      tileColors[106][0] = color;
      tileColors[114][0] = color;
      tileColors[128][0] = color;
      tileColors[139][0] = color;
      tileColors[172][0] = color;
      tileColors[216][0] = color;
      tileColors[269][0] = color;
      tileColors[334][0] = color;
      tileColors[471][0] = color;
      tileColors[470][0] = color;
      tileColors[475][0] = color;
      tileColors[377][0] = color;
      tileColors[380][0] = color;
      tileColors[395][0] = color;
      tileColors[573][0] = color;
      tileColors[12][0] = Color.FromRgb(174, 24, 69);
      tileColors[13][0] = Color.FromRgb(133, 213, 247);
      color = Color.FromRgb(144, 148, 144);
      tileColors[17][0] = color;
      tileColors[90][0] = color;
      tileColors[96][0] = color;
      tileColors[97][0] = color;
      tileColors[99][0] = color;
      tileColors[132][0] = color;
      tileColors[142][0] = color;
      tileColors[143][0] = color;
      tileColors[144][0] = color;
      tileColors[207][0] = color;
      tileColors[209][0] = color;
      tileColors[212][0] = color;
      tileColors[217][0] = color;
      tileColors[218][0] = color;
      tileColors[219][0] = color;
      tileColors[220][0] = color;
      tileColors[228][0] = color;
      tileColors[300][0] = color;
      tileColors[301][0] = color;
      tileColors[302][0] = color;
      tileColors[303][0] = color;
      tileColors[304][0] = color;
      tileColors[305][0] = color;
      tileColors[306][0] = color;
      tileColors[307][0] = color;
      tileColors[308][0] = color;
      tileColors[567][0] = color;
      tileColors[349][0] = Color.FromRgb(144, 148, 144);
      tileColors[531][0] = Color.FromRgb(144, 148, 144);
      tileColors[105][0] = Color.FromRgb(144, 148, 144);
      tileColors[105][1] = Color.FromRgb(177, 92, 31);
      tileColors[105][2] = Color.FromRgb(201, 188, 170);
      tileColors[137][0] = Color.FromRgb(144, 148, 144);
      tileColors[137][1] = Color.FromRgb(141, 56, 0);
      tileColors[16][0] = Color.FromRgb(140, 130, 116);
      tileColors[26][0] = Color.FromRgb(119, 101, 125);
      tileColors[26][1] = Color.FromRgb(214, 127, 133);
      tileColors[36][0] = Color.FromRgb(230, 89, 92);
      tileColors[28][0] = Color.FromRgb(151, 79, 80);
      tileColors[28][1] = Color.FromRgb(90, 139, 140);
      tileColors[28][2] = Color.FromRgb(192, 136, 70);
      tileColors[28][3] = Color.FromRgb(203, 185, 151);
      tileColors[28][4] = Color.FromRgb(73, 56, 41);
      tileColors[28][5] = Color.FromRgb(148, 159, 67);
      tileColors[28][6] = Color.FromRgb(138, 172, 67);
      tileColors[28][7] = Color.FromRgb(226, 122, 47);
      tileColors[28][8] = Color.FromRgb(198, 87, 93);
      tileColors[29][0] = Color.FromRgb(175, 105, 128);
      tileColors[51][0] = Color.FromRgb(192, 202, 203);
      tileColors[31][0] = Color.FromRgb(141, 120, 168);
      tileColors[31][1] = Color.FromRgb(212, 105, 105);
      tileColors[32][0] = Color.FromRgb(151, 135, 183);
      tileColors[42][0] = Color.FromRgb(251, 235, 127);
      tileColors[50][0] = Color.FromRgb(170, 48, 114);
      tileColors[85][0] = Color.FromRgb(192, 192, 192);
      tileColors[69][0] = Color.FromRgb(190, 150, 92);
      tileColors[77][0] = Color.FromRgb(238, 85, 70);
      tileColors[81][0] = Color.FromRgb(245, 133, 191);
      tileColors[78][0] = Color.FromRgb(121, 110, 97);
      tileColors[141][0] = Color.FromRgb(192, 59, 59);
      tileColors[129][0] = Color.FromRgb(255, 117, 224);
      tileColors[126][0] = Color.FromRgb(159, 209, 229);
      tileColors[125][0] = Color.FromRgb(141, 175, 255);
      tileColors[103][0] = Color.FromRgb(141, 98, 77);
      tileColors[95][0] = Color.FromRgb(255, 162, 31);
      tileColors[92][0] = Color.FromRgb(213, 229, 237);
      tileColors[91][0] = Color.FromRgb(13, 88, 130);
      tileColors[215][0] = Color.FromRgb(254, 121, 2);
      tileColors[592][0] = Color.FromRgb(254, 121, 2);
      tileColors[316][0] = Color.FromRgb(157, 176, 226);
      tileColors[317][0] = Color.FromRgb(118, 227, 129);
      tileColors[318][0] = Color.FromRgb(227, 118, 215);
      tileColors[319][0] = Color.FromRgb(96, 68, 48);
      tileColors[320][0] = Color.FromRgb(203, 185, 151);
      tileColors[321][0] = Color.FromRgb(96, 77, 64);
      tileColors[574][0] = Color.FromRgb(76, 57, 44);
      tileColors[322][0] = Color.FromRgb(198, 170, 104);
      tileColors[149][0] = Color.FromRgb(220, 50, 50);
      tileColors[149][1] = Color.FromRgb(0, 220, 50);
      tileColors[149][2] = Color.FromRgb(50, 50, 220);
      tileColors[133][0] = Color.FromRgb(231, 53, 56);
      tileColors[133][1] = Color.FromRgb(192, 189, 221);
      tileColors[134][0] = Color.FromRgb(166, 187, 153);
      tileColors[134][1] = Color.FromRgb(241, 129, 249);
      tileColors[102][0] = Color.FromRgb(229, 212, 73);
      tileColors[49][0] = Color.FromRgb(89, 201, 255);
      tileColors[35][0] = Color.FromRgb(226, 145, 30);
      tileColors[34][0] = Color.FromRgb(235, 166, 135);
      tileColors[136][0] = Color.FromRgb(213, 203, 204);
      tileColors[231][0] = Color.FromRgb(224, 194, 101);
      tileColors[239][0] = Color.FromRgb(224, 194, 101);
      tileColors[240][0] = Color.FromRgb(120, 85, 60);
      tileColors[240][1] = Color.FromRgb(99, 50, 30);
      tileColors[240][2] = Color.FromRgb(153, 153, 117);
      tileColors[240][3] = Color.FromRgb(112, 84, 56);
      tileColors[240][4] = Color.FromRgb(234, 231, 226);
      tileColors[241][0] = Color.FromRgb(77, 74, 72);
      tileColors[244][0] = Color.FromRgb(200, 245, 253);
      color = Color.FromRgb(99, 50, 30);
      tileColors[242][0] = color;
      tileColors[245][0] = color;
      tileColors[246][0] = color;
      tileColors[242][1] = Color.FromRgb(185, 142, 97);
      tileColors[247][0] = Color.FromRgb(140, 150, 150);
      tileColors[271][0] = Color.FromRgb(107, 250, 255);
      tileColors[270][0] = Color.FromRgb(187, 255, 107);
      tileColors[581][0] = Color.FromRgb(255, 150, 150);
      tileColors[572][0] = Color.FromRgb(255, 186, 212);
      tileColors[572][1] = Color.FromRgb(209, 201, 255);
      tileColors[572][2] = Color.FromRgb(200, 254, 255);
      tileColors[572][3] = Color.FromRgb(199, 255, 211);
      tileColors[572][4] = Color.FromRgb(180, 209, 255);
      tileColors[572][5] = Color.FromRgb(255, 220, 214);
      tileColors[314][0] = Color.FromRgb(181, 164, 125);
      tileColors[324][0] = Color.FromRgb(228, 213, 173);
      tileColors[351][0] = Color.FromRgb(31, 31, 31);
      tileColors[424][0] = Color.FromRgb(146, 155, 187);
      tileColors[429][0] = Color.FromRgb(220, 220, 220);
      tileColors[445][0] = Color.FromRgb(240, 240, 240);
      tileColors[21][0] = Color.FromRgb(174, 129, 92);
      tileColors[21][1] = Color.FromRgb(233, 207, 94);
      tileColors[21][2] = Color.FromRgb(137, 128, 200);
      tileColors[21][3] = Color.FromRgb(160, 160, 160);
      tileColors[21][4] = Color.FromRgb(106, 210, 255);
      tileColors[441][0] = tileColors[21][0];
      tileColors[441][1] = tileColors[21][1];
      tileColors[441][2] = tileColors[21][2];
      tileColors[441][3] = tileColors[21][3];
      tileColors[441][4] = tileColors[21][4];
      tileColors[27][0] = Color.FromRgb(54, 154, 54);
      tileColors[27][1] = Color.FromRgb(226, 196, 49);
      color = Color.FromRgb(246, 197, 26);
      tileColors[82][0] = color;
      tileColors[83][0] = color;
      tileColors[84][0] = color;
      color = Color.FromRgb(76, 150, 216);
      tileColors[82][1] = color;
      tileColors[83][1] = color;
      tileColors[84][1] = color;
      color = Color.FromRgb(185, 214, 42);
      tileColors[82][2] = color;
      tileColors[83][2] = color;
      tileColors[84][2] = color;
      color = Color.FromRgb(167, 203, 37);
      tileColors[82][3] = color;
      tileColors[83][3] = color;
      tileColors[84][3] = color;
      tileColors[591][6] = color;
      color = Color.FromRgb(32, 168, 117);
      tileColors[82][4] = color;
      tileColors[83][4] = color;
      tileColors[84][4] = color;
      color = Color.FromRgb(177, 69, 49);
      tileColors[82][5] = color;
      tileColors[83][5] = color;
      tileColors[84][5] = color;
      color = Color.FromRgb(40, 152, 240);
      tileColors[82][6] = color;
      tileColors[83][6] = color;
      tileColors[84][6] = color;
      tileColors[591][1] = Color.FromRgb(246, 197, 26);
      tileColors[591][2] = Color.FromRgb(76, 150, 216);
      tileColors[591][3] = Color.FromRgb(32, 168, 117);
      tileColors[591][4] = Color.FromRgb(40, 152, 240);
      tileColors[591][5] = Color.FromRgb(114, 81, 56);
      tileColors[591][6] = Color.FromRgb(141, 137, 223);
      tileColors[591][7] = Color.FromRgb(208, 80, 80);
      tileColors[591][8] = Color.FromRgb(177, 69, 49);
      tileColors[165][0] = Color.FromRgb(115, 173, 229);
      tileColors[165][1] = Color.FromRgb(100, 100, 100);
      tileColors[165][2] = Color.FromRgb(152, 152, 152);
      tileColors[165][3] = Color.FromRgb(227, 125, 22);
      tileColors[178][0] = Color.FromRgb(208, 94, 201);
      tileColors[178][1] = Color.FromRgb(233, 146, 69);
      tileColors[178][2] = Color.FromRgb(71, 146, 251);
      tileColors[178][3] = Color.FromRgb(60, 226, 133);
      tileColors[178][4] = Color.FromRgb(250, 30, 71);
      tileColors[178][5] = Color.FromRgb(166, 176, 204);
      tileColors[178][6] = Color.FromRgb(255, 217, 120);
      color = Color.FromRgb(99, 99, 99);
      tileColors[185][0] = color;
      tileColors[186][0] = color;
      tileColors[187][0] = color;
      tileColors[565][0] = color;
      tileColors[579][0] = color;
      color = Color.FromRgb(114, 81, 56);
      tileColors[185][1] = color;
      tileColors[186][1] = color;
      tileColors[187][1] = color;
      tileColors[591][0] = color;
      color = Color.FromRgb(133, 133, 101);
      tileColors[185][2] = color;
      tileColors[186][2] = color;
      tileColors[187][2] = color;
      color = Color.FromRgb(151, 200, 211);
      tileColors[185][3] = color;
      tileColors[186][3] = color;
      tileColors[187][3] = color;
      color = Color.FromRgb(177, 183, 161);
      tileColors[185][4] = color;
      tileColors[186][4] = color;
      tileColors[187][4] = color;
      color = Color.FromRgb(134, 114, 38);
      tileColors[185][5] = color;
      tileColors[186][5] = color;
      tileColors[187][5] = color;
      color = Color.FromRgb(82, 62, 66);
      tileColors[185][6] = color;
      tileColors[186][6] = color;
      tileColors[187][6] = color;
      color = Color.FromRgb(143, 117, 121);
      tileColors[185][7] = color;
      tileColors[186][7] = color;
      tileColors[187][7] = color;
      color = Color.FromRgb(177, 92, 31);
      tileColors[185][8] = color;
      tileColors[186][8] = color;
      tileColors[187][8] = color;
      color = Color.FromRgb(85, 73, 87);
      tileColors[185][9] = color;
      tileColors[186][9] = color;
      tileColors[187][9] = color;
      color = Color.FromRgb(26, 196, 84);
      tileColors[185][10] = color;
      tileColors[186][10] = color;
      tileColors[187][10] = color;
      tileColors[227][0] = Color.FromRgb(74, 197, 155);
      tileColors[227][1] = Color.FromRgb(54, 153, 88);
      tileColors[227][2] = Color.FromRgb(63, 126, 207);
      tileColors[227][3] = Color.FromRgb(240, 180, 4);
      tileColors[227][4] = Color.FromRgb(45, 68, 168);
      tileColors[227][5] = Color.FromRgb(61, 92, 0);
      tileColors[227][6] = Color.FromRgb(216, 112, 152);
      tileColors[227][7] = Color.FromRgb(200, 40, 24);
      tileColors[227][8] = Color.FromRgb(113, 45, 133);
      tileColors[227][9] = Color.FromRgb(235, 137, 2);
      tileColors[227][10] = Color.FromRgb(41, 152, 135);
      tileColors[227][11] = Color.FromRgb(198, 19, 78);
      tileColors[373][0] = Color.FromRgb(9, 61, 191);
      tileColors[374][0] = Color.FromRgb(253, 32, 3);
      tileColors[375][0] = Color.FromRgb(255, 156, 12);
      tileColors[461][0] = Color.FromRgb(212, 192, 100);
      tileColors[461][1] = Color.FromRgb(137, 132, 156);
      tileColors[461][2] = Color.FromRgb(148, 122, 112);
      tileColors[461][3] = Color.FromRgb(221, 201, 206);
      tileColors[323][0] = Color.FromRgb(182, 141, 86);
      tileColors[325][0] = Color.FromRgb(129, 125, 93);
      tileColors[326][0] = Color.FromRgb(9, 61, 191);
      tileColors[327][0] = Color.FromRgb(253, 32, 3);
      tileColors[507][0] = Color.FromRgb(5, 5, 5);
      tileColors[508][0] = Color.FromRgb(5, 5, 5);
      tileColors[330][0] = Color.FromRgb(226, 118, 76);
      tileColors[331][0] = Color.FromRgb(161, 172, 173);
      tileColors[332][0] = Color.FromRgb(204, 181, 72);
      tileColors[333][0] = Color.FromRgb(190, 190, 178);
      tileColors[335][0] = Color.FromRgb(217, 174, 137);
      tileColors[336][0] = Color.FromRgb(253, 62, 3);
      tileColors[337][0] = Color.FromRgb(144, 148, 144);
      tileColors[338][0] = Color.FromRgb(85, 255, 160);
      tileColors[315][0] = Color.FromRgb(235, 114, 80);
      tileColors[340][0] = Color.FromRgb(96, 248, 2);
      tileColors[341][0] = Color.FromRgb(105, 74, 202);
      tileColors[342][0] = Color.FromRgb(29, 240, 255);
      tileColors[343][0] = Color.FromRgb(254, 202, 80);
      tileColors[344][0] = Color.FromRgb(131, 252, 245);
      tileColors[345][0] = Color.FromRgb(255, 156, 12);
      tileColors[346][0] = Color.FromRgb(149, 212, 89);
      tileColors[347][0] = Color.FromRgb(236, 74, 79);
      tileColors[348][0] = Color.FromRgb(44, 26, 233);
      tileColors[350][0] = Color.FromRgb(55, 97, 155);
      tileColors[352][0] = Color.FromRgb(238, 97, 94);
      tileColors[354][0] = Color.FromRgb(141, 107, 89);
      tileColors[355][0] = Color.FromRgb(141, 107, 89);
      tileColors[463][0] = Color.FromRgb(155, 214, 240);
      tileColors[491][0] = Color.FromRgb(60, 20, 160);
      tileColors[464][0] = Color.FromRgb(233, 183, 128);
      tileColors[465][0] = Color.FromRgb(51, 84, 195);
      tileColors[466][0] = Color.FromRgb(205, 153, 73);
      tileColors[356][0] = Color.FromRgb(233, 203, 24);
      tileColors[357][0] = Color.FromRgb(168, 178, 204);
      tileColors[367][0] = Color.FromRgb(168, 178, 204);
      tileColors[561][0] = Color.FromRgb(148, 158, 184);
      tileColors[365][0] = Color.FromRgb(146, 136, 205);
      tileColors[366][0] = Color.FromRgb(223, 232, 233);
      tileColors[368][0] = Color.FromRgb(50, 46, 104);
      tileColors[369][0] = Color.FromRgb(50, 46, 104);
      tileColors[576][0] = Color.FromRgb(30, 26, 84);
      tileColors[370][0] = Color.FromRgb(127, 116, 194);
      tileColors[372][0] = Color.FromRgb(252, 128, 201);
      tileColors[371][0] = Color.FromRgb(249, 101, 189);
      tileColors[376][0] = Color.FromRgb(160, 120, 92);
      tileColors[378][0] = Color.FromRgb(160, 120, 100);
      tileColors[379][0] = Color.FromRgb(251, 209, 240);
      tileColors[382][0] = Color.FromRgb(28, 216, 94);
      tileColors[383][0] = Color.FromRgb(221, 136, 144);
      tileColors[384][0] = Color.FromRgb(131, 206, 12);
      tileColors[385][0] = Color.FromRgb(87, 21, 144);
      tileColors[386][0] = Color.FromRgb(127, 92, 69);
      tileColors[387][0] = Color.FromRgb(127, 92, 69);
      tileColors[388][0] = Color.FromRgb(127, 92, 69);
      tileColors[389][0] = Color.FromRgb(127, 92, 69);
      tileColors[390][0] = Color.FromRgb(253, 32, 3);
      tileColors[397][0] = Color.FromRgb(212, 192, 100);
      tileColors[396][0] = Color.FromRgb(198, 124, 78);
      tileColors[577][0] = Color.FromRgb(178, 104, 58);
      tileColors[398][0] = Color.FromRgb(100, 82, 126);
      tileColors[399][0] = Color.FromRgb(77, 76, 66);
      tileColors[400][0] = Color.FromRgb(96, 68, 117);
      tileColors[401][0] = Color.FromRgb(68, 60, 51);
      tileColors[402][0] = Color.FromRgb(174, 168, 186);
      tileColors[403][0] = Color.FromRgb(205, 152, 186);
      tileColors[404][0] = Color.FromRgb(212, 148, 88);
      tileColors[405][0] = Color.FromRgb(140, 140, 140);
      tileColors[406][0] = Color.FromRgb(120, 120, 120);
      tileColors[407][0] = Color.FromRgb(255, 227, 132);
      tileColors[411][0] = Color.FromRgb(227, 46, 46);
      tileColors[494][0] = Color.FromRgb(227, 227, 227);
      tileColors[421][0] = Color.FromRgb(65, 75, 90);
      tileColors[422][0] = Color.FromRgb(65, 75, 90);
      tileColors[425][0] = Color.FromRgb(146, 155, 187);
      tileColors[426][0] = Color.FromRgb(168, 38, 47);
      tileColors[430][0] = Color.FromRgb(39, 168, 96);
      tileColors[431][0] = Color.FromRgb(39, 94, 168);
      tileColors[432][0] = Color.FromRgb(242, 221, 100);
      tileColors[433][0] = Color.FromRgb(224, 100, 242);
      tileColors[434][0] = Color.FromRgb(197, 193, 216);
      tileColors[427][0] = Color.FromRgb(183, 53, 62);
      tileColors[435][0] = Color.FromRgb(54, 183, 111);
      tileColors[436][0] = Color.FromRgb(54, 109, 183);
      tileColors[437][0] = Color.FromRgb(255, 236, 115);
      tileColors[438][0] = Color.FromRgb(239, 115, 255);
      tileColors[439][0] = Color.FromRgb(212, 208, 231);
      tileColors[440][0] = Color.FromRgb(238, 51, 53);
      tileColors[440][1] = Color.FromRgb(13, 107, 216);
      tileColors[440][2] = Color.FromRgb(33, 184, 115);
      tileColors[440][3] = Color.FromRgb(255, 221, 62);
      tileColors[440][4] = Color.FromRgb(165, 0, 236);
      tileColors[440][5] = Color.FromRgb(223, 230, 238);
      tileColors[440][6] = Color.FromRgb(207, 101, 0);
      tileColors[419][0] = Color.FromRgb(88, 95, 114);
      tileColors[419][1] = Color.FromRgb(214, 225, 236);
      tileColors[419][2] = Color.FromRgb(25, 131, 205);
      tileColors[423][0] = Color.FromRgb(245, 197, 1);
      tileColors[423][1] = Color.FromRgb(185, 0, 224);
      tileColors[423][2] = Color.FromRgb(58, 240, 111);
      tileColors[423][3] = Color.FromRgb(50, 107, 197);
      tileColors[423][4] = Color.FromRgb(253, 91, 3);
      tileColors[423][5] = Color.FromRgb(254, 194, 20);
      tileColors[423][6] = Color.FromRgb(174, 195, 215);
      tileColors[420][0] = Color.FromRgb(99, 255, 107);
      tileColors[420][1] = Color.FromRgb(99, 255, 107);
      tileColors[420][4] = Color.FromRgb(99, 255, 107);
      tileColors[420][2] = Color.FromRgb(218, 2, 5);
      tileColors[420][3] = Color.FromRgb(218, 2, 5);
      tileColors[420][5] = Color.FromRgb(218, 2, 5);
      tileColors[476][0] = Color.FromRgb(160, 160, 160);
      tileColors[410][0] = Color.FromRgb(75, 139, 166);
      tileColors[480][0] = Color.FromRgb(120, 50, 50);
      tileColors[509][0] = Color.FromRgb(50, 50, 60);
      tileColors[412][0] = Color.FromRgb(75, 139, 166);
      tileColors[443][0] = Color.FromRgb(144, 148, 144);
      tileColors[442][0] = Color.FromRgb(3, 144, 201);
      tileColors[444][0] = Color.FromRgb(191, 176, 124);
      tileColors[446][0] = Color.FromRgb(255, 66, 152);
      tileColors[447][0] = Color.FromRgb(179, 132, 255);
      tileColors[448][0] = Color.FromRgb(0, 206, 180);
      tileColors[449][0] = Color.FromRgb(91, 186, 240);
      tileColors[450][0] = Color.FromRgb(92, 240, 91);
      tileColors[451][0] = Color.FromRgb(240, 91, 147);
      tileColors[452][0] = Color.FromRgb(255, 150, 181);
      tileColors[453][0] = Color.FromRgb(179, 132, 255);
      tileColors[453][1] = Color.FromRgb(0, 206, 180);
      tileColors[453][2] = Color.FromRgb(255, 66, 152);
      tileColors[454][0] = Color.FromRgb(174, 16, 176);
      tileColors[455][0] = Color.FromRgb(48, 225, 110);
      tileColors[456][0] = Color.FromRgb(179, 132, 255);
      tileColors[457][0] = Color.FromRgb(150, 164, 206);
      tileColors[457][1] = Color.FromRgb(255, 132, 184);
      tileColors[457][2] = Color.FromRgb(74, 255, 232);
      tileColors[457][3] = Color.FromRgb(215, 159, 255);
      tileColors[457][4] = Color.FromRgb(229, 219, 234);
      tileColors[458][0] = Color.FromRgb(211, 198, 111);
      tileColors[459][0] = Color.FromRgb(190, 223, 232);
      tileColors[460][0] = Color.FromRgb(141, 163, 181);
      tileColors[462][0] = Color.FromRgb(231, 178, 28);
      tileColors[467][0] = Color.FromRgb(129, 56, 121);
      tileColors[467][1] = Color.FromRgb(255, 249, 59);
      tileColors[467][2] = Color.FromRgb(161, 67, 24);
      tileColors[467][3] = Color.FromRgb(89, 70, 72);
      tileColors[467][4] = Color.FromRgb(233, 207, 94);
      tileColors[467][5] = Color.FromRgb(254, 158, 35);
      tileColors[467][6] = Color.FromRgb(34, 221, 151);
      tileColors[467][7] = Color.FromRgb(249, 170, 236);
      tileColors[467][8] = Color.FromRgb(35, 200, 254);
      tileColors[467][9] = Color.FromRgb(190, 200, 200);
      tileColors[467][10] = Color.FromRgb(230, 170, 100);
      tileColors[467][11] = Color.FromRgb(165, 168, 26);
      for (int j = 0; j < 12; j++)
      {
        tileColors[468][j] = tileColors[467][j];
      }
      tileColors[472][0] = Color.FromRgb(190, 160, 140);
      tileColors[473][0] = Color.FromRgb(85, 114, 123);
      tileColors[474][0] = Color.FromRgb(116, 94, 97);
      tileColors[478][0] = Color.FromRgb(108, 34, 35);
      tileColors[479][0] = Color.FromRgb(178, 114, 68);
      tileColors[485][0] = Color.FromRgb(198, 134, 88);
      tileColors[492][0] = Color.FromRgb(78, 193, 227);
      tileColors[492][0] = Color.FromRgb(78, 193, 227);
      tileColors[493][0] = Color.FromRgb(250, 249, 252);
      tileColors[493][1] = Color.FromRgb(240, 90, 90);
      tileColors[493][2] = Color.FromRgb(98, 230, 92);
      tileColors[493][3] = Color.FromRgb(95, 197, 238);
      tileColors[493][4] = Color.FromRgb(241, 221, 100);
      tileColors[493][5] = Color.FromRgb(213, 92, 237);
      tileColors[494][0] = Color.FromRgb(224, 219, 236);
      tileColors[495][0] = Color.FromRgb(253, 227, 215);
      tileColors[496][0] = Color.FromRgb(165, 159, 153);
      tileColors[498][0] = Color.FromRgb(202, 174, 165);
      tileColors[499][0] = Color.FromRgb(160, 187, 142);
      tileColors[500][0] = Color.FromRgb(254, 158, 35);
      tileColors[501][0] = Color.FromRgb(34, 221, 151);
      tileColors[502][0] = Color.FromRgb(249, 170, 236);
      tileColors[503][0] = Color.FromRgb(35, 200, 254);
      tileColors[506][0] = Color.FromRgb(61, 61, 61);
      tileColors[510][0] = Color.FromRgb(191, 142, 111);
      tileColors[511][0] = Color.FromRgb(187, 68, 74);
      tileColors[520][0] = Color.FromRgb(224, 219, 236);
      tileColors[545][0] = Color.FromRgb(255, 126, 145);
      tileColors[530][0] = Color.FromRgb(107, 182, 0);
      tileColors[530][1] = Color.FromRgb(23, 154, 209);
      tileColors[530][2] = Color.FromRgb(238, 97, 94);
      tileColors[530][3] = Color.FromRgb(113, 108, 205);
      tileColors[546][0] = Color.FromRgb(60, 60, 60);
      tileColors[557][0] = Color.FromRgb(60, 60, 60);
      tileColors[547][0] = Color.FromRgb(120, 110, 100);
      tileColors[548][0] = Color.FromRgb(120, 110, 100);
      tileColors[562][0] = Color.FromRgb(165, 168, 26);
      tileColors[563][0] = Color.FromRgb(165, 168, 26);
      tileColors[571][0] = Color.FromRgb(165, 168, 26);
      tileColors[568][0] = Color.FromRgb(248, 203, 233);
      tileColors[569][0] = Color.FromRgb(203, 248, 218);
      tileColors[570][0] = Color.FromRgb(160, 242, 255);
      tileColors[597][0] = Color.FromRgb(28, 216, 94);
      tileColors[597][1] = Color.FromRgb(183, 237, 20);
      tileColors[597][2] = Color.FromRgb(185, 83, 200);
      tileColors[597][3] = Color.FromRgb(131, 128, 168);
      tileColors[597][4] = Color.FromRgb(38, 142, 214);
      tileColors[597][5] = Color.FromRgb(229, 154, 9);
      tileColors[597][6] = Color.FromRgb(142, 227, 234);
      tileColors[597][7] = Color.FromRgb(98, 111, 223);
      tileColors[597][8] = Color.FromRgb(241, 233, 158);
      tileColors[617][0] = Color.FromRgb(233, 207, 94);
      Color color3 = Color.FromRgb(250, 100, 50);
      tileColors[548][1] = color3;
      tileColors[613][0] = color3;
      tileColors[614][0] = color3;
      Color[] array2 = new Color[]
      {
        Color.FromRgb(9, 61, 191),
        Color.FromRgb(253, 32, 3),
        Color.FromRgb(254, 194, 20)
      };
      for (int k = 0; k < 316; k++)
      {
        wallColors[k] = new Color[2];
      }
      wallColors[158][0] = Color.FromRgb(107, 49, 154);
      wallColors[163][0] = Color.FromRgb(154, 148, 49);
      wallColors[162][0] = Color.FromRgb(49, 49, 154);
      wallColors[160][0] = Color.FromRgb(49, 154, 68);
      wallColors[161][0] = Color.FromRgb(154, 49, 77);
      wallColors[159][0] = Color.FromRgb(85, 89, 118);
      wallColors[157][0] = Color.FromRgb(154, 83, 49);
      wallColors[154][0] = Color.FromRgb(221, 79, 255);
      wallColors[166][0] = Color.FromRgb(250, 255, 79);
      wallColors[165][0] = Color.FromRgb(79, 102, 255);
      wallColors[156][0] = Color.FromRgb(79, 255, 89);
      wallColors[164][0] = Color.FromRgb(255, 79, 79);
      wallColors[155][0] = Color.FromRgb(240, 240, 247);
      wallColors[153][0] = Color.FromRgb(255, 145, 79);
      wallColors[169][0] = Color.FromRgb(5, 5, 5);
      wallColors[224][0] = Color.FromRgb(57, 55, 52);
      wallColors[225][0] = Color.FromRgb(68, 68, 68);
      wallColors[226][0] = Color.FromRgb(148, 138, 74);
      wallColors[227][0] = Color.FromRgb(95, 137, 191);
      wallColors[170][0] = Color.FromRgb(59, 39, 22);
      wallColors[171][0] = Color.FromRgb(59, 39, 22);
      color = Color.FromRgb(52, 52, 52);
      wallColors[1][0] = color;
      wallColors[53][0] = color;
      wallColors[52][0] = color;
      wallColors[51][0] = color;
      wallColors[50][0] = color;
      wallColors[49][0] = color;
      wallColors[48][0] = color;
      wallColors[44][0] = color;
      wallColors[5][0] = color;
      color = Color.FromRgb(88, 61, 46);
      wallColors[2][0] = color;
      wallColors[16][0] = color;
      wallColors[59][0] = color;
      wallColors[3][0] = Color.FromRgb(61, 58, 78);
      wallColors[4][0] = Color.FromRgb(73, 51, 36);
      wallColors[6][0] = Color.FromRgb(91, 30, 30);
      color = Color.FromRgb(27, 31, 42);
      wallColors[7][0] = color;
      wallColors[17][0] = color;
      color = Color.FromRgb(32, 40, 45);
      wallColors[94][0] = color;
      wallColors[100][0] = color;
      color = Color.FromRgb(44, 41, 50);
      wallColors[95][0] = color;
      wallColors[101][0] = color;
      color = Color.FromRgb(31, 39, 26);
      wallColors[8][0] = color;
      wallColors[18][0] = color;
      color = Color.FromRgb(36, 45, 44);
      wallColors[98][0] = color;
      wallColors[104][0] = color;
      color = Color.FromRgb(38, 49, 50);
      wallColors[99][0] = color;
      wallColors[105][0] = color;
      color = Color.FromRgb(41, 28, 36);
      wallColors[9][0] = color;
      wallColors[19][0] = color;
      color = Color.FromRgb(72, 50, 77);
      wallColors[96][0] = color;
      wallColors[102][0] = color;
      color = Color.FromRgb(78, 50, 69);
      wallColors[97][0] = color;
      wallColors[103][0] = color;
      wallColors[10][0] = Color.FromRgb(74, 62, 12);
      wallColors[11][0] = Color.FromRgb(46, 56, 59);
      wallColors[12][0] = Color.FromRgb(75, 32, 11);
      wallColors[13][0] = Color.FromRgb(67, 37, 37);
      color = Color.FromRgb(15, 15, 15);
      wallColors[14][0] = color;
      wallColors[20][0] = color;
      wallColors[15][0] = Color.FromRgb(52, 43, 45);
      wallColors[22][0] = Color.FromRgb(113, 99, 99);
      wallColors[23][0] = Color.FromRgb(38, 38, 43);
      wallColors[24][0] = Color.FromRgb(53, 39, 41);
      wallColors[25][0] = Color.FromRgb(11, 35, 62);
      wallColors[26][0] = Color.FromRgb(21, 63, 70);
      wallColors[27][0] = Color.FromRgb(88, 61, 46);
      wallColors[27][1] = Color.FromRgb(52, 52, 52);
      wallColors[28][0] = Color.FromRgb(81, 84, 101);
      wallColors[29][0] = Color.FromRgb(88, 23, 23);
      wallColors[30][0] = Color.FromRgb(28, 88, 23);
      wallColors[31][0] = Color.FromRgb(78, 87, 99);
      color = Color.FromRgb(69, 67, 41);
      wallColors[34][0] = color;
      wallColors[37][0] = color;
      wallColors[32][0] = Color.FromRgb(86, 17, 40);
      wallColors[33][0] = Color.FromRgb(49, 47, 83);
      wallColors[35][0] = Color.FromRgb(51, 51, 70);
      wallColors[36][0] = Color.FromRgb(87, 59, 55);
      wallColors[38][0] = Color.FromRgb(49, 57, 49);
      wallColors[39][0] = Color.FromRgb(78, 79, 73);
      wallColors[45][0] = Color.FromRgb(60, 59, 51);
      wallColors[46][0] = Color.FromRgb(48, 57, 47);
      wallColors[47][0] = Color.FromRgb(71, 77, 85);
      wallColors[40][0] = Color.FromRgb(85, 102, 103);
      wallColors[41][0] = Color.FromRgb(52, 50, 62);
      wallColors[42][0] = Color.FromRgb(71, 42, 44);
      wallColors[43][0] = Color.FromRgb(73, 66, 50);
      wallColors[54][0] = Color.FromRgb(40, 56, 50);
      wallColors[55][0] = Color.FromRgb(49, 48, 36);
      wallColors[56][0] = Color.FromRgb(43, 33, 32);
      wallColors[57][0] = Color.FromRgb(31, 40, 49);
      wallColors[58][0] = Color.FromRgb(48, 35, 52);
      wallColors[60][0] = Color.FromRgb(1, 52, 20);
      wallColors[61][0] = Color.FromRgb(55, 39, 26);
      wallColors[62][0] = Color.FromRgb(39, 33, 26);
      wallColors[69][0] = Color.FromRgb(43, 42, 68);
      wallColors[70][0] = Color.FromRgb(30, 70, 80);
      color = Color.FromRgb(30, 80, 48);
      wallColors[63][0] = color;
      wallColors[65][0] = color;
      wallColors[66][0] = color;
      wallColors[68][0] = color;
      color = Color.FromRgb(53, 80, 30);
      wallColors[64][0] = color;
      wallColors[67][0] = color;
      wallColors[78][0] = Color.FromRgb(63, 39, 26);
      wallColors[244][0] = Color.FromRgb(63, 39, 26);
      wallColors[71][0] = Color.FromRgb(78, 105, 135);
      wallColors[72][0] = Color.FromRgb(52, 84, 12);
      wallColors[73][0] = Color.FromRgb(190, 204, 223);
      color = Color.FromRgb(64, 62, 80);
      wallColors[74][0] = color;
      wallColors[80][0] = color;
      wallColors[75][0] = Color.FromRgb(65, 65, 35);
      wallColors[76][0] = Color.FromRgb(20, 46, 104);
      wallColors[77][0] = Color.FromRgb(61, 13, 16);
      wallColors[79][0] = Color.FromRgb(51, 47, 96);
      wallColors[81][0] = Color.FromRgb(101, 51, 51);
      wallColors[82][0] = Color.FromRgb(77, 64, 34);
      wallColors[83][0] = Color.FromRgb(62, 38, 41);
      wallColors[234][0] = Color.FromRgb(60, 36, 39);
      wallColors[84][0] = Color.FromRgb(48, 78, 93);
      wallColors[85][0] = Color.FromRgb(54, 63, 69);
      color = Color.FromRgb(138, 73, 38);
      wallColors[86][0] = color;
      wallColors[108][0] = color;
      color = Color.FromRgb(50, 15, 8);
      wallColors[87][0] = color;
      wallColors[112][0] = color;
      wallColors[109][0] = Color.FromRgb(94, 25, 17);
      wallColors[110][0] = Color.FromRgb(125, 36, 122);
      wallColors[111][0] = Color.FromRgb(51, 35, 27);
      wallColors[113][0] = Color.FromRgb(135, 58, 0);
      wallColors[114][0] = Color.FromRgb(65, 52, 15);
      wallColors[115][0] = Color.FromRgb(39, 42, 51);
      wallColors[116][0] = Color.FromRgb(89, 26, 27);
      wallColors[117][0] = Color.FromRgb(126, 123, 115);
      wallColors[118][0] = Color.FromRgb(8, 50, 19);
      wallColors[119][0] = Color.FromRgb(95, 21, 24);
      wallColors[120][0] = Color.FromRgb(17, 31, 65);
      wallColors[121][0] = Color.FromRgb(192, 173, 143);
      wallColors[122][0] = Color.FromRgb(114, 114, 131);
      wallColors[123][0] = Color.FromRgb(136, 119, 7);
      wallColors[124][0] = Color.FromRgb(8, 72, 3);
      wallColors[125][0] = Color.FromRgb(117, 132, 82);
      wallColors[126][0] = Color.FromRgb(100, 102, 114);
      wallColors[127][0] = Color.FromRgb(30, 118, 226);
      wallColors[128][0] = Color.FromRgb(93, 6, 102);
      wallColors[129][0] = Color.FromRgb(64, 40, 169);
      wallColors[130][0] = Color.FromRgb(39, 34, 180);
      wallColors[131][0] = Color.FromRgb(87, 94, 125);
      wallColors[132][0] = Color.FromRgb(6, 6, 6);
      wallColors[133][0] = Color.FromRgb(69, 72, 186);
      wallColors[134][0] = Color.FromRgb(130, 62, 16);
      wallColors[135][0] = Color.FromRgb(22, 123, 163);
      wallColors[136][0] = Color.FromRgb(40, 86, 151);
      wallColors[137][0] = Color.FromRgb(183, 75, 15);
      wallColors[138][0] = Color.FromRgb(83, 80, 100);
      wallColors[139][0] = Color.FromRgb(115, 65, 68);
      wallColors[140][0] = Color.FromRgb(119, 108, 81);
      wallColors[141][0] = Color.FromRgb(59, 67, 71);
      wallColors[142][0] = Color.FromRgb(17, 172, 143);
      wallColors[143][0] = Color.FromRgb(90, 112, 105);
      wallColors[144][0] = Color.FromRgb(62, 28, 87);
      wallColors[146][0] = Color.FromRgb(120, 59, 19);
      wallColors[147][0] = Color.FromRgb(59, 59, 59);
      wallColors[148][0] = Color.FromRgb(229, 218, 161);
      wallColors[149][0] = Color.FromRgb(73, 59, 50);
      wallColors[151][0] = Color.FromRgb(102, 75, 34);
      wallColors[167][0] = Color.FromRgb(70, 68, 51);
      wallColors[172][0] = Color.FromRgb(163, 96, 0);
      wallColors[242][0] = Color.FromRgb(5, 5, 5);
      wallColors[243][0] = Color.FromRgb(5, 5, 5);
      wallColors[173][0] = Color.FromRgb(94, 163, 46);
      wallColors[174][0] = Color.FromRgb(117, 32, 59);
      wallColors[175][0] = Color.FromRgb(20, 11, 203);
      wallColors[176][0] = Color.FromRgb(74, 69, 88);
      wallColors[177][0] = Color.FromRgb(60, 30, 30);
      wallColors[183][0] = Color.FromRgb(111, 117, 135);
      wallColors[179][0] = Color.FromRgb(111, 117, 135);
      wallColors[178][0] = Color.FromRgb(111, 117, 135);
      wallColors[184][0] = Color.FromRgb(25, 23, 54);
      wallColors[181][0] = Color.FromRgb(25, 23, 54);
      wallColors[180][0] = Color.FromRgb(25, 23, 54);
      wallColors[182][0] = Color.FromRgb(74, 71, 129);
      wallColors[185][0] = Color.FromRgb(52, 52, 52);
      wallColors[186][0] = Color.FromRgb(38, 9, 66);
      wallColors[216][0] = Color.FromRgb(158, 100, 64);
      wallColors[217][0] = Color.FromRgb(62, 45, 75);
      wallColors[218][0] = Color.FromRgb(57, 14, 12);
      wallColors[219][0] = Color.FromRgb(96, 72, 133);
      wallColors[187][0] = Color.FromRgb(149, 80, 51);
      wallColors[235][0] = Color.FromRgb(140, 75, 48);
      wallColors[220][0] = Color.FromRgb(67, 55, 80);
      wallColors[221][0] = Color.FromRgb(64, 37, 29);
      wallColors[222][0] = Color.FromRgb(70, 51, 91);
      wallColors[188][0] = Color.FromRgb(82, 63, 80);
      wallColors[189][0] = Color.FromRgb(65, 61, 77);
      wallColors[190][0] = Color.FromRgb(64, 65, 92);
      wallColors[191][0] = Color.FromRgb(76, 53, 84);
      wallColors[192][0] = Color.FromRgb(144, 67, 52);
      wallColors[193][0] = Color.FromRgb(149, 48, 48);
      wallColors[194][0] = Color.FromRgb(111, 32, 36);
      wallColors[195][0] = Color.FromRgb(147, 48, 55);
      wallColors[196][0] = Color.FromRgb(97, 67, 51);
      wallColors[197][0] = Color.FromRgb(112, 80, 62);
      wallColors[198][0] = Color.FromRgb(88, 61, 46);
      wallColors[199][0] = Color.FromRgb(127, 94, 76);
      wallColors[200][0] = Color.FromRgb(143, 50, 123);
      wallColors[201][0] = Color.FromRgb(136, 120, 131);
      wallColors[202][0] = Color.FromRgb(219, 92, 143);
      wallColors[203][0] = Color.FromRgb(113, 64, 150);
      wallColors[204][0] = Color.FromRgb(74, 67, 60);
      wallColors[205][0] = Color.FromRgb(60, 78, 59);
      wallColors[206][0] = Color.FromRgb(0, 54, 21);
      wallColors[207][0] = Color.FromRgb(74, 97, 72);
      wallColors[208][0] = Color.FromRgb(40, 37, 35);
      wallColors[209][0] = Color.FromRgb(77, 63, 66);
      wallColors[210][0] = Color.FromRgb(111, 6, 6);
      wallColors[211][0] = Color.FromRgb(88, 67, 59);
      wallColors[212][0] = Color.FromRgb(88, 87, 80);
      wallColors[213][0] = Color.FromRgb(71, 71, 67);
      wallColors[214][0] = Color.FromRgb(76, 52, 60);
      wallColors[215][0] = Color.FromRgb(89, 48, 59);
      wallColors[223][0] = Color.FromRgb(51, 18, 4);
      wallColors[228][0] = Color.FromRgb(160, 2, 75);
      wallColors[229][0] = Color.FromRgb(100, 55, 164);
      wallColors[230][0] = Color.FromRgb(0, 117, 101);
      wallColors[236][0] = Color.FromRgb(127, 49, 44);
      wallColors[231][0] = Color.FromRgb(110, 90, 78);
      wallColors[232][0] = Color.FromRgb(47, 69, 75);
      wallColors[233][0] = Color.FromRgb(91, 67, 70);
      wallColors[237][0] = Color.FromRgb(200, 44, 18);
      wallColors[238][0] = Color.FromRgb(24, 93, 66);
      wallColors[239][0] = Color.FromRgb(160, 87, 234);
      wallColors[240][0] = Color.FromRgb(6, 106, 255);
      wallColors[245][0] = Color.FromRgb(102, 102, 102);
      wallColors[315][0] = Color.FromRgb(181, 230, 29);
      wallColors[246][0] = Color.FromRgb(61, 58, 78);
      wallColors[247][0] = Color.FromRgb(52, 43, 45);
      wallColors[248][0] = Color.FromRgb(81, 84, 101);
      wallColors[249][0] = Color.FromRgb(85, 102, 103);
      wallColors[250][0] = Color.FromRgb(52, 52, 52);
      wallColors[251][0] = Color.FromRgb(52, 52, 52);
      wallColors[252][0] = Color.FromRgb(52, 52, 52);
      wallColors[253][0] = Color.FromRgb(52, 52, 52);
      wallColors[254][0] = Color.FromRgb(52, 52, 52);
      wallColors[255][0] = Color.FromRgb(52, 52, 52);
      wallColors[314][0] = Color.FromRgb(52, 52, 52);
      wallColors[256][0] = Color.FromRgb(40, 56, 50);
      wallColors[257][0] = Color.FromRgb(49, 48, 36);
      wallColors[258][0] = Color.FromRgb(43, 33, 32);
      wallColors[259][0] = Color.FromRgb(31, 40, 49);
      wallColors[260][0] = Color.FromRgb(48, 35, 52);
      wallColors[261][0] = Color.FromRgb(88, 61, 46);
      wallColors[262][0] = Color.FromRgb(55, 39, 26);
      wallColors[263][0] = Color.FromRgb(39, 33, 26);
      wallColors[264][0] = Color.FromRgb(43, 42, 68);
      wallColors[265][0] = Color.FromRgb(30, 70, 80);
      wallColors[266][0] = Color.FromRgb(78, 105, 135);
      wallColors[267][0] = Color.FromRgb(51, 47, 96);
      wallColors[268][0] = Color.FromRgb(101, 51, 51);
      wallColors[269][0] = Color.FromRgb(62, 38, 41);
      wallColors[270][0] = Color.FromRgb(59, 39, 22);
      wallColors[271][0] = Color.FromRgb(59, 39, 22);
      wallColors[272][0] = Color.FromRgb(111, 117, 135);
      wallColors[273][0] = Color.FromRgb(25, 23, 54);
      wallColors[274][0] = Color.FromRgb(52, 52, 52);
      wallColors[275][0] = Color.FromRgb(149, 80, 51);
      wallColors[276][0] = Color.FromRgb(82, 63, 80);
      wallColors[277][0] = Color.FromRgb(65, 61, 77);
      wallColors[278][0] = Color.FromRgb(64, 65, 92);
      wallColors[279][0] = Color.FromRgb(76, 53, 84);
      wallColors[280][0] = Color.FromRgb(144, 67, 52);
      wallColors[281][0] = Color.FromRgb(149, 48, 48);
      wallColors[282][0] = Color.FromRgb(111, 32, 36);
      wallColors[283][0] = Color.FromRgb(147, 48, 55);
      wallColors[284][0] = Color.FromRgb(97, 67, 51);
      wallColors[285][0] = Color.FromRgb(112, 80, 62);
      wallColors[286][0] = Color.FromRgb(88, 61, 46);
      wallColors[287][0] = Color.FromRgb(127, 94, 76);
      wallColors[288][0] = Color.FromRgb(143, 50, 123);
      wallColors[289][0] = Color.FromRgb(136, 120, 131);
      wallColors[290][0] = Color.FromRgb(219, 92, 143);
      wallColors[291][0] = Color.FromRgb(113, 64, 150);
      wallColors[292][0] = Color.FromRgb(74, 67, 60);
      wallColors[293][0] = Color.FromRgb(60, 78, 59);
      wallColors[294][0] = Color.FromRgb(0, 54, 21);
      wallColors[295][0] = Color.FromRgb(74, 97, 72);
      wallColors[296][0] = Color.FromRgb(40, 37, 35);
      wallColors[297][0] = Color.FromRgb(77, 63, 66);
      wallColors[298][0] = Color.FromRgb(111, 6, 6);
      wallColors[299][0] = Color.FromRgb(88, 67, 59);
      wallColors[300][0] = Color.FromRgb(88, 87, 80);
      wallColors[301][0] = Color.FromRgb(71, 71, 67);
      wallColors[302][0] = Color.FromRgb(76, 52, 60);
      wallColors[303][0] = Color.FromRgb(89, 48, 59);
      wallColors[304][0] = Color.FromRgb(158, 100, 64);
      wallColors[305][0] = Color.FromRgb(62, 45, 75);
      wallColors[306][0] = Color.FromRgb(57, 14, 12);
      wallColors[307][0] = Color.FromRgb(96, 72, 133);
      wallColors[308][0] = Color.FromRgb(67, 55, 80);
      wallColors[309][0] = Color.FromRgb(64, 37, 29);
      wallColors[310][0] = Color.FromRgb(70, 51, 91);
      wallColors[311][0] = Color.FromRgb(51, 18, 4);
      wallColors[312][0] = Color.FromRgb(78, 110, 51);
      wallColors[313][0] = Color.FromRgb(78, 110, 51);
      Color[] array4 = new Color[256];
      Color color4 = Color.FromRgb(50, 40, 255);
      Color color5 = Color.FromRgb(145, 185, 255);
      for (int l = 0; l < array4.Length; l++)
      {
        float num = (float)l / (float)array4.Length;
        float num2 = 1f - num;
        array4[l] = Color.FromRgb(((byte)((float)color4.R * num2 + (float)color5.R * num)), ((byte)((float)color4.G * num2 + (float)color5.G * num)), ((byte)((float)color4.B * num2 + (float)color5.B * num)));
      }
      Color[] array5 = new Color[256];
      Color color6 = Color.FromRgb(88, 61, 46);
      Color color7 = Color.FromRgb(37, 78, 123);
      for (int m = 0; m < array5.Length; m++)
      {
        float num3 = (float)m / 255f;
        float num4 = 1f - num3;
        array5[m] = Color.FromRgb(((byte)((float)color6.R * num4 + (float)color7.R * num3)), ((byte)((float)color6.G * num4 + (float)color7.G * num3)), ((byte)((float)color6.B * num4 + (float)color7.B * num3)));
      }
      Color[] array6 = new Color[256];
      Color color8 = Color.FromRgb(74, 67, 60);
      color7 = Color.FromRgb(53, 70, 97);
      for (int n = 0; n < array6.Length; n++)
      {
        float num5 = (float)n / 255f;
        float num6 = 1f - num5;
        array6[n] = Color.FromRgb(((byte)((float)color8.R * num6 + (float)color7.R * num5)), ((byte)((float)color8.G * num6 + (float)color7.G * num5)), ((byte)((float)color8.B * num6 + (float)color7.B * num5)));
      }
      Color color9 = Color.FromRgb(50, 44, 38);
      int num7 = 0;
      MapHelper.tileOptionCounts = new int[623];
      for (int num8 = 0; num8 < 623; num8++)
      {
        Color[] array7 = tileColors[num8];
        int num9 = 0;
        while (num9 < 12 && !(array7[num9] == Colors.Transparent))
        {
          num9++;
        }
        MapHelper.tileOptionCounts[num8] = num9;
        num7 += num9;
      }
      MapHelper.wallOptionCounts = new int[316];
      for (int num10 = 0; num10 < 316; num10++)
      {
        Color[] array8 = wallColors[num10];
        int num11 = 0;
        while (num11 < 2 && !(array8[num11] == Colors.Transparent))
        {
          num11++;
        }
        MapHelper.wallOptionCounts[num10] = num11;
        num7 += num11;
      }
      num7 += 773;
      MapHelper.colorLookup = new Color[num7];
      MapHelper.colorLookup[0] = Colors.Transparent;
      ushort num12 = 1;
      MapHelper.tilePosition = num12;
      MapHelper.tileLookup = new ushort[623];
      for (int num13 = 0; num13 < 623; num13++)
      {
        if (MapHelper.tileOptionCounts[num13] > 0)
        {
          Color[] arg_6CA0_0 = tileColors[num13];
          MapHelper.tileLookup[num13] = num12;
          for (int num14 = 0; num14 < MapHelper.tileOptionCounts[num13]; num14++)
          {
            MapHelper.colorLookup[(int)num12] = tileColors[num13][num14];
            num12 += 1;
          }
        }
        else
        {
          MapHelper.tileLookup[num13] = 0;
        }
      }
      MapHelper.wallPosition = num12;
      MapHelper.wallLookup = new ushort[316];
      MapHelper.wallRangeStart = num12;
      for (int num15 = 0; num15 < 316; num15++)
      {
        if (MapHelper.wallOptionCounts[num15] > 0)
        {
          Color[] arg_6D2B_0 = wallColors[num15];
          MapHelper.wallLookup[num15] = num12;
          for (int num16 = 0; num16 < MapHelper.wallOptionCounts[num15]; num16++)
          {
            MapHelper.colorLookup[(int)num12] = wallColors[num15][num16];
            num12 += 1;
          }
        }
        else
        {
          MapHelper.wallLookup[num15] = 0;
        }
      }
      MapHelper.wallRangeEnd = num12;
      MapHelper.liquidPosition = num12;
      for (int num17 = 0; num17 < 3; num17++)
      {
        MapHelper.colorLookup[(int)num12] = array2[num17];
        num12 += 1;
      }
      MapHelper.skyPosition = num12;
      for (int num18 = 0; num18 < 256; num18++)
      {
        MapHelper.colorLookup[(int)num12] = array4[num18];
        num12 += 1;
      }
      MapHelper.dirtPosition = num12;
      for (int num19 = 0; num19 < 256; num19++)
      {
        MapHelper.colorLookup[(int)num12] = array5[num19];
        num12 += 1;
      }
      MapHelper.rockPosition = num12;
      for (int num20 = 0; num20 < 256; num20++)
      {
        MapHelper.colorLookup[(int)num12] = array6[num20];
        num12 += 1;
      }
      MapHelper.hellPosition = num12;
      MapHelper.colorLookup[(int)num12] = color9;
      MapHelper.snowTypes = new ushort[6];
      MapHelper.snowTypes[0] = MapHelper.tileLookup[147];
      MapHelper.snowTypes[1] = MapHelper.tileLookup[161];
      MapHelper.snowTypes[2] = MapHelper.tileLookup[162];
      MapHelper.snowTypes[3] = MapHelper.tileLookup[163];
      MapHelper.snowTypes[4] = MapHelper.tileLookup[164];
      MapHelper.snowTypes[5] = MapHelper.tileLookup[200];
    }

    public static Color GetTileColor(ushort tileType, short u, short v)
    {
      if (tileType >= tileColors.Length)
        return Colors.Black;

      var iu = u / 34;

      if (iu < tileColors[tileType].Length && tileColors[tileType][iu].A > 0)
        return tileColors[tileType][iu];

      return tileColors[tileType][0];
    }

    public static Color GetWallColor(ushort wallType)
    {
      return wallColors[wallType][0];
    }

    public static Color GetLiquidColor(ushort liquidType)
    {
      return LiquidColors[liquidType];
    }

    private static byte[][] tileLight;

    public static Color[] LiquidColors { get; }

    public static bool IsTileLit(int x, int y)
    {
      return GetTileLight(x, y) > 0;
    }

    public static void ResetTileLight()
    {
      tileLight = null;
    }

    public static byte GetTileLight(int x, int y)
    {
      byte light = 0;

      if (tileLight != null)
        light = tileLight[x][y];

      return light;
    }

    // Terraria.Map.MapHelper
    public static void LoadMapVersion2(BinaryReader fileIO, World world)
    {
      var version = fileIO.ReadInt32();

      ulong check = fileIO.ReadUInt64();
      if ((check & 72057594037927935uL) != 27981915666277746uL)
      {
        throw new FileFormatException("Expected Re-Logic file format.");
      }

      var revision = fileIO.ReadUInt32();
      ulong favorite = fileIO.ReadUInt64();
      var isFavorite = ((favorite & 1uL) == 1uL);

      string a = fileIO.ReadString();
      int num = fileIO.ReadInt32();
      int num2 = fileIO.ReadInt32();
      int num3 = fileIO.ReadInt32();
      if (a != world.Name || num != world.Id || num3 != world.WorldWidthinTiles || num2 != world.WorldHeightinTiles)
      {
        throw new Exception("Map meta-data is invalid.");
      }

      tileLight = new byte[world.WorldWidthinTiles][];
      for (int i = 0; i < world.WorldWidthinTiles; i++)
      {
        tileLight[i] = new byte[world.WorldHeightinTiles];
      }

      short num4 = fileIO.ReadInt16();
      short num5 = fileIO.ReadInt16();
      short num6 = fileIO.ReadInt16();
      short num7 = fileIO.ReadInt16();
      short num8 = fileIO.ReadInt16();
      short num9 = fileIO.ReadInt16();
      bool[] array = new bool[(int)num4];
      byte b = 0;
      byte b2 = 128;
      for (int i = 0; i < (int)num4; i++)
      {
        if (b2 == 128)
        {
          b = fileIO.ReadByte();
          b2 = 1;
        }
        else
        {
          b2 = (byte)(b2 << 1);
        }
        if ((b & b2) == b2)
        {
          array[i] = true;
        }
      }
      bool[] array2 = new bool[(int)num5];
      b = 0;
      b2 = 128;
      for (int i = 0; i < (int)num5; i++)
      {
        if (b2 == 128)
        {
          b = fileIO.ReadByte();
          b2 = 1;
        }
        else
        {
          b2 = (byte)(b2 << 1);
        }
        if ((b & b2) == b2)
        {
          array2[i] = true;
        }
      }
      byte[] array3 = new byte[(int)num4];
      ushort num10 = 0;
      for (int i = 0; i < (int)num4; i++)
      {
        if (array[i])
        {
          array3[i] = fileIO.ReadByte();
        }
        else
        {
          array3[i] = 1;
        }
        num10 += (ushort)array3[i];
      }
      byte[] array4 = new byte[(int)num5];
      ushort num11 = 0;
      for (int i = 0; i < (int)num5; i++)
      {
        if (array2[i])
        {
          array4[i] = fileIO.ReadByte();
        }
        else
        {
          array4[i] = 1;
        }
        num11 += (ushort)array4[i];
      }
      int num12 = (int)(num10 + num11 + (ushort)num6 + (ushort)num7 + (ushort)num8 + (ushort)num9 + 2);
      ushort[] array5 = new ushort[num12];
      array5[0] = 0;
      ushort num13 = 1;
      ushort num14 = 1;
      ushort num15 = num14;
      for (int i = 0; i < tileOptionCounts.Length; i++)
      {
        if (i < (int)num4)
        {
          int num16 = (int)array3[i];
          int num17 = MapHelper.tileOptionCounts[i];
          for (int j = 0; j < num17; j++)
          {
            if (j < num16)
            {
              array5[(int)num14] = num13;
              num14 += 1;
            }
            num13 += 1;
          }
        }
        else
        {
          num13 += (ushort)MapHelper.tileOptionCounts[i];
        }
      }
      ushort num18 = num14;
      for (int i = 0; i < wallOptionCounts.Length; i++)
      {
        if (i < (int)num5)
        {
          int num19 = (int)array4[i];
          int num20 = MapHelper.wallOptionCounts[i];
          for (int k = 0; k < num20; k++)
          {
            if (k < num19)
            {
              array5[(int)num14] = num13;
              num14 += 1;
            }
            num13 += 1;
          }
        }
        else
        {
          num13 += (ushort)MapHelper.wallOptionCounts[i];
        }
      }
      ushort num21 = num14;
      for (int i = 0; i < 3; i++)
      {
        if (i < (int)num6)
        {
          array5[(int)num14] = num13;
          num14 += 1;
        }
        num13 += 1;
      }
      ushort num22 = num14;
      for (int i = 0; i < 256; i++)
      {
        if (i < (int)num7)
        {
          array5[(int)num14] = num13;
          num14 += 1;
        }
        num13 += 1;
      }
      ushort num23 = num14;
      for (int i = 0; i < 256; i++)
      {
        if (i < (int)num8)
        {
          array5[(int)num14] = num13;
          num14 += 1;
        }
        num13 += 1;
      }
      ushort num24 = num14;
      for (int i = 0; i < 256; i++)
      {
        if (i < (int)num9)
        {
          array5[(int)num14] = num13;
          num14 += 1;
        }
        num13 += 1;
      }
      ushort num25 = num14;
      array5[(int)num14] = num13;
      BinaryReader binaryReader;

      DeflateStream input = new DeflateStream(fileIO.BaseStream, CompressionMode.Decompress);
      binaryReader = new BinaryReader(input);

      for (int y = 0; y < world.WorldHeightinTiles; y++)
      {
        float num26 = (float)y / (float)world.WorldHeightinTiles;

        for (int x = 0; x < world.WorldWidthinTiles; x++)
        {
          byte b3 = binaryReader.ReadByte();
          byte b4;
          if ((b3 & 1) == 1)
          {
            b4 = binaryReader.ReadByte();
          }
          else
          {
            b4 = 0;
          }
          byte b5 = (byte)((b3 & 14) >> 1);
          bool flag;
          switch (b5)
          {
            case 0:
              flag = false;
              break;
            case 1:
            case 2:
            case 7:
              flag = true;
              break;
            case 3:
            case 4:
            case 5:
              flag = false;
              break;
            case 6:
              flag = false;
              break;
            default:
              flag = false;
              break;
          }
          ushort num27;
          if (flag)
          {
            if ((b3 & 16) == 16)
            {
              num27 = binaryReader.ReadUInt16();
            }
            else
            {
              num27 = (ushort)binaryReader.ReadByte();
            }
          }
          else
          {
            num27 = 0;
          }
          byte b6;
          if ((b3 & 32) == 32)
          {
            b6 = binaryReader.ReadByte();
          }
          else
          {
            b6 = 255;
          }
          int n;
          switch ((byte)((b3 & 192) >> 6))
          {
            case 0:
              n = 0;
              break;
            case 1:
              n = (int)binaryReader.ReadByte();
              break;
            case 2:
              n = (int)binaryReader.ReadInt16();
              break;
            default:
              n = 0;
              break;
          }
          if (b5 == 0)
          {
            x += n;
          }
          else
          {
            switch (b5)
            {
              case 1:
                num27 += num15;
                break;
              case 2:
                num27 += num18;
                break;
              case 3:
              case 4:
              case 5:
                num27 = (ushort)(num27 + num21 + (ushort)(b5 - 3));
                break;
              case 6:
                if ((double)y < world.WorldSurfaceY)
                {
                  ushort num28 = (ushort)((double)num7 * ((double)y / world.WorldSurfaceY));
                  num27 = (ushort)(num27 + num22 + num28);
                }
                else
                {
                  num27 = num25;
                }
                break;
              case 7:
                if ((double)y < world.RockLayerY)
                {
                  num27 += num23;
                }
                else
                {
                  num27 += num24;
                }
                break;
            }

            tileLight[x][y] = b6;

            //MapTile mapTile = MapTile.Create(array5[(int)num27], b6, (byte)(b4 >> 1 & 31));
            //Main.Map.SetTile(x, y, ref mapTile);
            if (b6 == 255)
            {
              while (n > 0)
              {
                x++;
                //Main.Map.SetTile(x, y, ref mapTile);
                tileLight[x][y] = 255;
                n--;
              }
            }
            else
            {
              while (n > 0)
              {
                x++;
                var light = binaryReader.ReadByte();
                tileLight[x][y] = light;
                //mapTile = mapTile.WithLight(binaryReader.ReadByte());
                //Main.Map.SetTile(x, y, ref mapTile);
                n--;
              }
            }
          }
        }
      }
      binaryReader.Close();
    }
  }
}