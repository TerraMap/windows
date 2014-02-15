using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	// copied from Terraria.Map
	public static class Map
	{
		static Color[][] tileColors = new Color[314][];
		static Color[] liquidColors;
		static Color[][] wallColors;

		public static int[] tileOptionCounts;
		public static int[] wallOptionCounts;
		public static ushort[] tileLookup;
		public static ushort[] wallLookup;
		public static ushort tilePosition;
		public static ushort wallPosition;
		public static ushort liquidPosition;
		public static ushort skyPosition;
		public static ushort dirtPosition;
		public static ushort rockPosition;
		public static ushort hellPosition;
		public static Color[] colorLookup;
		public static ushort[] snowTypes;

		public static void Initialize()
		{
			for (int i = 0; i < 314; i++)
			{
				tileColors[i] = new Color[10];
			}
			Color color = Color.FromArgb(151, 107, 75);
			tileColors[0][0] = color;
			tileColors[5][0] = color;
			tileColors[30][0] = color;
			tileColors[191][0] = color;
			tileColors[272][0] = Color.FromArgb(121, 119, 101);
			color = Color.FromArgb(128, 128, 128);
			tileColors[1][0] = color;
			tileColors[38][0] = color;
			tileColors[48][0] = color;
			tileColors[130][0] = color;
			tileColors[138][0] = color;
			tileColors[273][0] = color;
			tileColors[283][0] = color;
			tileColors[2][0] = Color.FromArgb(28, 216, 94);
			color = Color.FromArgb(26, 196, 84);
			tileColors[3][0] = color;
			tileColors[192][0] = color;
			tileColors[73][0] = Color.FromArgb(27, 197, 109);
			tileColors[52][0] = Color.FromArgb(23, 177, 76);
			tileColors[20][0] = Color.FromArgb(163, 116, 81);
			tileColors[6][0] = Color.FromArgb(140, 101, 80);
			color = Color.FromArgb(150, 67, 22);
			tileColors[7][0] = color;
			tileColors[47][0] = color;
			tileColors[284][0] = color;
			color = Color.FromArgb(185, 164, 23);
			tileColors[8][0] = color;
			tileColors[45][0] = color;
			color = Color.FromArgb(185, 194, 195);
			tileColors[9][0] = color;
			tileColors[46][0] = color;
			color = Color.FromArgb(98, 95, 167);
			tileColors[22][0] = color;
			tileColors[140][0] = color;
			tileColors[23][0] = Color.FromArgb(141, 137, 223);
			tileColors[24][0] = Color.FromArgb(122, 116, 218);
			tileColors[25][0] = Color.FromArgb(109, 90, 128);
			tileColors[37][0] = Color.FromArgb(104, 86, 84);
			tileColors[39][0] = Color.FromArgb(181, 62, 59);
			tileColors[40][0] = Color.FromArgb(146, 81, 68);
			tileColors[41][0] = Color.FromArgb(66, 84, 109);
			tileColors[43][0] = Color.FromArgb(84, 100, 63);
			tileColors[44][0] = Color.FromArgb(107, 68, 99);
			tileColors[53][0] = Color.FromArgb(186, 168, 84);
			color = Color.FromArgb(190, 171, 94);
			tileColors[151][0] = color;
			tileColors[154][0] = color;
			tileColors[274][0] = color;
			tileColors[54][0] = Color.FromArgb(200, 246, 254);
			tileColors[56][0] = Color.FromArgb(43, 40, 84);
			tileColors[75][0] = Color.FromArgb(26, 26, 26);
			tileColors[57][0] = Color.FromArgb(68, 68, 76);
			color = Color.FromArgb(142, 66, 66);
			tileColors[58][0] = color;
			tileColors[76][0] = color;
			color = Color.FromArgb(92, 68, 73);
			tileColors[59][0] = color;
			tileColors[120][0] = color;
			tileColors[60][0] = Color.FromArgb(143, 215, 29);
			tileColors[61][0] = Color.FromArgb(135, 196, 26);
			tileColors[74][0] = Color.FromArgb(96, 197, 27);
			tileColors[62][0] = Color.FromArgb(121, 176, 24);
			tileColors[233][0] = Color.FromArgb(107, 182, 29);
			tileColors[63][0] = Color.FromArgb(110, 140, 182);
			tileColors[64][0] = Color.FromArgb(196, 96, 114);
			tileColors[65][0] = Color.FromArgb(56, 150, 97);
			tileColors[66][0] = Color.FromArgb(160, 118, 58);
			tileColors[67][0] = Color.FromArgb(140, 58, 166);
			tileColors[68][0] = Color.FromArgb(125, 191, 197);
			tileColors[70][0] = Color.FromArgb(93, 127, 255);
			color = Color.FromArgb(182, 175, 130);
			tileColors[71][0] = color;
			tileColors[72][0] = color;
			tileColors[190][0] = color;
			color = Color.FromArgb(73, 120, 17);
			tileColors[80][0] = color;
			tileColors[188][0] = color;
			color = Color.FromArgb(11, 80, 143);
			tileColors[107][0] = color;
			tileColors[121][0] = color;
			color = Color.FromArgb(91, 169, 169);
			tileColors[108][0] = color;
			tileColors[122][0] = color;
			color = Color.FromArgb(128, 26, 52);
			tileColors[111][0] = color;
			tileColors[150][0] = color;
			tileColors[109][0] = Color.FromArgb(78, 193, 227);
			tileColors[110][0] = Color.FromArgb(48, 186, 135);
			tileColors[113][0] = Color.FromArgb(48, 208, 234);
			tileColors[115][0] = Color.FromArgb(33, 171, 207);
			tileColors[112][0] = Color.FromArgb(103, 98, 122);
			color = Color.FromArgb(238, 225, 218);
			tileColors[116][0] = color;
			tileColors[118][0] = color;
			tileColors[117][0] = Color.FromArgb(181, 172, 190);
			tileColors[119][0] = Color.FromArgb(107, 92, 108);
			tileColors[123][0] = Color.FromArgb(106, 107, 118);
			tileColors[124][0] = Color.FromArgb(73, 51, 36);
			tileColors[131][0] = Color.FromArgb(52, 52, 52);
			tileColors[145][0] = Color.FromArgb(192, 30, 30);
			tileColors[146][0] = Color.FromArgb(43, 192, 30);
			color = Color.FromArgb(211, 236, 241);
			tileColors[147][0] = color;
			tileColors[148][0] = color;
			tileColors[152][0] = Color.FromArgb(128, 133, 184);
			tileColors[153][0] = Color.FromArgb(239, 141, 126);
			tileColors[155][0] = Color.FromArgb(131, 162, 161);
			tileColors[156][0] = Color.FromArgb(170, 171, 157);
			tileColors[157][0] = Color.FromArgb(104, 100, 126);
			color = Color.FromArgb(145, 81, 85);
			tileColors[158][0] = color;
			tileColors[232][0] = color;
			tileColors[159][0] = Color.FromArgb(148, 133, 98);
			tileColors[160][0] = Color.FromArgb(200, 0, 0);
			tileColors[160][1] = Color.FromArgb(0, 200, 0);
			tileColors[160][2] = Color.FromArgb(0, 0, 200);
			tileColors[161][0] = Color.FromArgb(144, 195, 232);
			tileColors[162][0] = Color.FromArgb(184, 219, 240);
			tileColors[163][0] = Color.FromArgb(174, 145, 214);
			tileColors[164][0] = Color.FromArgb(218, 182, 204);
			tileColors[170][0] = Color.FromArgb(27, 109, 69);
			tileColors[171][0] = Color.FromArgb(33, 135, 85);
			color = Color.FromArgb(129, 125, 93);
			tileColors[166][0] = color;
			tileColors[175][0] = color;
			tileColors[167][0] = Color.FromArgb(62, 82, 114);
			color = Color.FromArgb(132, 157, 127);
			tileColors[168][0] = color;
			tileColors[176][0] = color;
			color = Color.FromArgb(152, 171, 198);
			tileColors[169][0] = color;
			tileColors[177][0] = color;
			tileColors[179][0] = Color.FromArgb(49, 134, 114);
			tileColors[180][0] = Color.FromArgb(126, 134, 49);
			tileColors[181][0] = Color.FromArgb(134, 59, 49);
			tileColors[182][0] = Color.FromArgb(43, 86, 140);
			tileColors[183][0] = Color.FromArgb(121, 49, 134);
			tileColors[189][0] = Color.FromArgb(223, 255, 255);
			tileColors[193][0] = Color.FromArgb(56, 121, 255);
			tileColors[194][0] = Color.FromArgb(157, 157, 107);
			tileColors[195][0] = Color.FromArgb(134, 22, 34);
			tileColors[196][0] = Color.FromArgb(147, 144, 178);
			tileColors[197][0] = Color.FromArgb(97, 200, 225);
			tileColors[198][0] = Color.FromArgb(62, 61, 52);
			tileColors[199][0] = Color.FromArgb(208, 80, 80);
			tileColors[201][0] = Color.FromArgb(203, 61, 64);
			tileColors[205][0] = Color.FromArgb(186, 50, 52);
			tileColors[200][0] = Color.FromArgb(216, 152, 144);
			tileColors[202][0] = Color.FromArgb(213, 178, 28);
			tileColors[203][0] = Color.FromArgb(128, 44, 45);
			tileColors[204][0] = Color.FromArgb(125, 55, 65);
			tileColors[206][0] = Color.FromArgb(124, 175, 201);
			tileColors[208][0] = Color.FromArgb(88, 105, 118);
			tileColors[211][0] = Color.FromArgb(191, 233, 115);
			tileColors[213][0] = Color.FromArgb(137, 120, 67);
			tileColors[214][0] = Color.FromArgb(103, 103, 103);
			tileColors[221][0] = Color.FromArgb(239, 90, 50);
			tileColors[222][0] = Color.FromArgb(231, 96, 228);
			tileColors[223][0] = Color.FromArgb(57, 85, 101);
			tileColors[224][0] = Color.FromArgb(107, 132, 139);
			tileColors[225][0] = Color.FromArgb(227, 125, 22);
			tileColors[226][0] = Color.FromArgb(141, 56, 0);
			tileColors[229][0] = Color.FromArgb(255, 156, 12);
			tileColors[230][0] = Color.FromArgb(131, 79, 13);
			tileColors[234][0] = Color.FromArgb(53, 44, 41);
			tileColors[235][0] = Color.FromArgb(214, 184, 46);
			tileColors[236][0] = Color.FromArgb(149, 232, 87);
			tileColors[237][0] = Color.FromArgb(255, 241, 51);
			tileColors[238][0] = Color.FromArgb(225, 128, 206);
			tileColors[243][0] = Color.FromArgb(198, 196, 170);
			tileColors[248][0] = Color.FromArgb(219, 71, 38);
			tileColors[249][0] = Color.FromArgb(235, 38, 231);
			tileColors[250][0] = Color.FromArgb(86, 85, 92);
			tileColors[251][0] = Color.FromArgb(235, 150, 23);
			tileColors[252][0] = Color.FromArgb(153, 131, 44);
			tileColors[253][0] = Color.FromArgb(57, 48, 97);
			tileColors[254][0] = Color.FromArgb(248, 158, 92);
			tileColors[255][0] = Color.FromArgb(107, 49, 154);
			tileColors[256][0] = Color.FromArgb(154, 148, 49);
			tileColors[257][0] = Color.FromArgb(49, 49, 154);
			tileColors[258][0] = Color.FromArgb(49, 154, 68);
			tileColors[259][0] = Color.FromArgb(154, 49, 77);
			tileColors[260][0] = Color.FromArgb(85, 89, 118);
			tileColors[261][0] = Color.FromArgb(154, 83, 49);
			tileColors[262][0] = Color.FromArgb(221, 79, 255);
			tileColors[263][0] = Color.FromArgb(250, 255, 79);
			tileColors[264][0] = Color.FromArgb(79, 102, 255);
			tileColors[265][0] = Color.FromArgb(79, 255, 89);
			tileColors[266][0] = Color.FromArgb(255, 79, 79);
			tileColors[267][0] = Color.FromArgb(240, 240, 247);
			tileColors[268][0] = Color.FromArgb(255, 145, 79);
			tileColors[287][0] = Color.FromArgb(79, 128, 17);
			color = Color.FromArgb(122, 217, 232);
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
			tileColors[311][0] = Color.FromArgb(117, 61, 25);
			tileColors[312][0] = Color.FromArgb(204, 93, 73);
			tileColors[313][0] = Color.FromArgb(87, 150, 154);
			tileColors[4][0] = Color.FromArgb(169, 125, 93);
			tileColors[4][1] = Color.FromArgb(253, 221, 3);
			color = Color.FromArgb(253, 221, 3);
			tileColors[93][0] = color;
			tileColors[33][0] = color;
			tileColors[174][0] = color;
			tileColors[100][0] = color;
			tileColors[98][0] = color;
			tileColors[173][0] = color;
			color = Color.FromArgb(119, 105, 79);
			tileColors[11][0] = color;
			tileColors[10][0] = color;
			color = Color.FromArgb(191, 142, 111);
			tileColors[14][0] = color;
			tileColors[15][0] = color;
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
			tileColors[216][0] = color;
			tileColors[269][0] = color;
			tileColors[12][0] = Color.FromArgb(174, 24, 69);
			tileColors[13][0] = Color.FromArgb(133, 213, 247);
			color = Color.FromArgb(144, 148, 144);
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
			tileColors[105][0] = Color.FromArgb(144, 148, 144);
			tileColors[105][1] = Color.FromArgb(177, 92, 31);
			tileColors[105][2] = Color.FromArgb(201, 188, 170);
			tileColors[137][0] = Color.FromArgb(144, 148, 144);
			tileColors[137][1] = Color.FromArgb(141, 56, 0);
			tileColors[16][0] = Color.FromArgb(140, 130, 116);
			tileColors[26][0] = Color.FromArgb(119, 101, 125);
			tileColors[26][1] = Color.FromArgb(214, 127, 133);
			tileColors[36][0] = Color.FromArgb(230, 89, 92);
			tileColors[28][0] = Color.FromArgb(151, 79, 80);
			tileColors[28][1] = Color.FromArgb(90, 139, 140);
			tileColors[28][2] = Color.FromArgb(192, 136, 70);
			tileColors[28][3] = Color.FromArgb(203, 185, 151);
			tileColors[28][4] = Color.FromArgb(73, 56, 41);
			tileColors[28][5] = Color.FromArgb(148, 159, 67);
			tileColors[28][6] = Color.FromArgb(138, 172, 67);
			tileColors[28][7] = Color.FromArgb(226, 122, 47);
			tileColors[28][8] = Color.FromArgb(198, 87, 93);
			tileColors[29][0] = Color.FromArgb(175, 105, 128);
			tileColors[51][0] = Color.FromArgb(192, 202, 203);
			tileColors[31][0] = Color.FromArgb(141, 120, 168);
			tileColors[31][1] = Color.FromArgb(212, 105, 105);
			tileColors[32][0] = Color.FromArgb(151, 135, 183);
			tileColors[42][0] = Color.FromArgb(251, 235, 127);
			tileColors[50][0] = Color.FromArgb(170, 48, 114);
			tileColors[85][0] = Color.FromArgb(192, 192, 192);
			tileColors[69][0] = Color.FromArgb(190, 150, 92);
			tileColors[77][0] = Color.FromArgb(238, 85, 70);
			tileColors[81][0] = Color.FromArgb(245, 133, 191);
			tileColors[78][0] = Color.FromArgb(121, 110, 97);
			tileColors[141][0] = Color.FromArgb(192, 59, 59);
			tileColors[129][0] = Color.FromArgb(255, 117, 224);
			tileColors[126][0] = Color.FromArgb(159, 209, 229);
			tileColors[125][0] = Color.FromArgb(141, 175, 255);
			tileColors[103][0] = Color.FromArgb(141, 98, 77);
			tileColors[95][0] = Color.FromArgb(255, 162, 31);
			tileColors[92][0] = Color.FromArgb(213, 229, 237);
			tileColors[91][0] = Color.FromArgb(13, 88, 130);
			tileColors[215][0] = Color.FromArgb(254, 121, 2);
			tileColors[149][0] = Color.FromArgb(220, 50, 50);
			tileColors[149][1] = Color.FromArgb(0, 220, 50);
			tileColors[149][2] = Color.FromArgb(50, 50, 220);
			tileColors[133][0] = Color.FromArgb(231, 53, 56);
			tileColors[133][1] = Color.FromArgb(192, 189, 221);
			tileColors[134][0] = Color.FromArgb(166, 187, 153);
			tileColors[134][1] = Color.FromArgb(241, 129, 249);
			tileColors[102][0] = Color.FromArgb(229, 212, 73);
			tileColors[49][0] = Color.FromArgb(89, 201, 255);
			tileColors[35][0] = Color.FromArgb(226, 145, 30);
			tileColors[34][0] = Color.FromArgb(235, 166, 135);
			tileColors[136][0] = Color.FromArgb(213, 203, 204);
			tileColors[231][0] = Color.FromArgb(224, 194, 101);
			tileColors[239][0] = Color.FromArgb(224, 194, 101);
			tileColors[240][0] = Color.FromArgb(120, 85, 60);
			tileColors[240][1] = Color.FromArgb(99, 50, 30);
			tileColors[240][2] = Color.FromArgb(153, 153, 117);
			tileColors[240][3] = Color.FromArgb(112, 84, 56);
			tileColors[241][0] = Color.FromArgb(77, 74, 72);
			tileColors[244][0] = Color.FromArgb(200, 245, 253);
			color = Color.FromArgb(99, 50, 30);
			tileColors[242][0] = color;
			tileColors[242][1] = Color.FromArgb(185, 142, 97);
			tileColors[245][0] = color;
			tileColors[246][0] = color;
			tileColors[247][0] = Color.FromArgb(140, 150, 150);
			tileColors[271][0] = Color.FromArgb(107, 250, 255);
			tileColors[270][0] = Color.FromArgb(187, 255, 107);
			tileColors[21][0] = Color.FromArgb(174, 129, 92);
			tileColors[21][1] = Color.FromArgb(233, 207, 94);
			tileColors[21][2] = Color.FromArgb(137, 128, 200);
			tileColors[21][3] = Color.FromArgb(160, 160, 160);
			tileColors[21][4] = Color.FromArgb(106, 210, 255);
			tileColors[27][0] = Color.FromArgb(54, 154, 54);
			tileColors[27][1] = Color.FromArgb(226, 196, 49);
			color = Color.FromArgb(246, 197, 26);
			tileColors[82][0] = color;
			tileColors[83][0] = color;
			tileColors[84][0] = color;
			color = Color.FromArgb(76, 150, 216);
			tileColors[82][1] = color;
			tileColors[83][1] = color;
			tileColors[84][1] = color;
			color = Color.FromArgb(185, 214, 42);
			tileColors[82][2] = color;
			tileColors[83][2] = color;
			tileColors[84][2] = color;
			color = Color.FromArgb(167, 203, 37);
			tileColors[82][3] = color;
			tileColors[83][3] = color;
			tileColors[84][3] = color;
			color = Color.FromArgb(72, 145, 125);
			tileColors[82][4] = color;
			tileColors[83][4] = color;
			tileColors[84][4] = color;
			color = Color.FromArgb(177, 69, 49);
			tileColors[82][5] = color;
			tileColors[83][5] = color;
			tileColors[84][5] = color;
			tileColors[165][0] = Color.FromArgb(115, 173, 229);
			tileColors[165][1] = Color.FromArgb(100, 100, 100);
			tileColors[165][2] = Color.FromArgb(152, 152, 152);
			tileColors[165][3] = Color.FromArgb(227, 125, 22);
			tileColors[178][0] = Color.FromArgb(208, 94, 201);
			tileColors[178][1] = Color.FromArgb(233, 146, 69);
			tileColors[178][2] = Color.FromArgb(71, 146, 251);
			tileColors[178][3] = Color.FromArgb(60, 226, 133);
			tileColors[178][4] = Color.FromArgb(250, 30, 71);
			tileColors[178][5] = Color.FromArgb(166, 176, 204);
			tileColors[178][6] = Color.FromArgb(255, 217, 120);
			tileColors[184][0] = Color.FromArgb(29, 106, 88);
			tileColors[184][1] = Color.FromArgb(94, 100, 36);
			tileColors[184][2] = Color.FromArgb(96, 44, 40);
			tileColors[184][3] = Color.FromArgb(34, 63, 102);
			tileColors[184][4] = Color.FromArgb(79, 35, 95);
			color = Color.FromArgb(99, 99, 99);
			tileColors[185][0] = color;
			tileColors[186][0] = color;
			tileColors[187][0] = color;
			color = Color.FromArgb(114, 81, 56);
			tileColors[185][1] = color;
			tileColors[186][1] = color;
			tileColors[187][1] = color;
			color = Color.FromArgb(133, 133, 101);
			tileColors[185][2] = color;
			tileColors[186][2] = color;
			tileColors[187][2] = color;
			color = Color.FromArgb(151, 200, 211);
			tileColors[185][3] = color;
			tileColors[186][3] = color;
			tileColors[187][3] = color;
			color = Color.FromArgb(177, 183, 161);
			tileColors[185][4] = color;
			tileColors[186][4] = color;
			tileColors[187][4] = color;
			color = Color.FromArgb(134, 114, 38);
			tileColors[185][5] = color;
			tileColors[186][5] = color;
			tileColors[187][5] = color;
			color = Color.FromArgb(82, 62, 66);
			tileColors[185][6] = color;
			tileColors[186][6] = color;
			tileColors[187][6] = color;
			color = Color.FromArgb(143, 117, 121);
			tileColors[185][7] = color;
			tileColors[186][7] = color;
			tileColors[187][7] = color;
			color = Color.FromArgb(177, 92, 31);
			tileColors[185][8] = color;
			tileColors[186][8] = color;
			tileColors[187][8] = color;
			color = Color.FromArgb(85, 73, 87);
			tileColors[185][9] = color;
			tileColors[186][9] = color;
			tileColors[187][9] = color;
			tileColors[227][0] = Color.FromArgb(74, 197, 155);
			tileColors[227][1] = Color.FromArgb(54, 153, 88);
			tileColors[227][2] = Color.FromArgb(63, 126, 207);
			tileColors[227][3] = Color.FromArgb(240, 180, 4);
			tileColors[227][4] = Color.FromArgb(45, 68, 168);
			tileColors[227][5] = Color.FromArgb(61, 92, 0);
			tileColors[227][6] = Color.FromArgb(216, 112, 152);
			tileColors[227][7] = Color.FromArgb(200, 40, 24);
			liquidColors = new Color[]
			{
				Color.FromArgb(9, 61, 191),
				Color.FromArgb(253, 32, 3),
				Color.FromArgb(254, 194, 20)
			};
			wallColors = new Color[145][];
			for (int j = 0; j < 145; j++)
			{
				wallColors[j] = new Color[2];
			}
			color = Color.FromArgb(52, 52, 52);
			wallColors[1][0] = color;
			wallColors[53][0] = color;
			wallColors[52][0] = color;
			wallColors[51][0] = color;
			wallColors[50][0] = color;
			wallColors[49][0] = color;
			wallColors[48][0] = color;
			wallColors[44][0] = color;
			wallColors[5][0] = color;
			color = Color.FromArgb(88, 61, 46);
			wallColors[2][0] = color;
			wallColors[16][0] = color;
			wallColors[59][0] = color;
			wallColors[3][0] = Color.FromArgb(61, 58, 78);
			wallColors[4][0] = Color.FromArgb(73, 51, 36);
			wallColors[6][0] = Color.FromArgb(91, 30, 30);
			color = Color.FromArgb(27, 31, 42);
			wallColors[7][0] = color;
			wallColors[17][0] = color;
			color = Color.FromArgb(32, 40, 45);
			wallColors[94][0] = color;
			wallColors[100][0] = color;
			color = Color.FromArgb(44, 41, 50);
			wallColors[95][0] = color;
			wallColors[101][0] = color;
			color = Color.FromArgb(31, 39, 26);
			wallColors[8][0] = color;
			wallColors[18][0] = color;
			color = Color.FromArgb(36, 45, 44);
			wallColors[98][0] = color;
			wallColors[104][0] = color;
			color = Color.FromArgb(38, 49, 50);
			wallColors[99][0] = color;
			wallColors[105][0] = color;
			color = Color.FromArgb(41, 28, 36);
			wallColors[9][0] = color;
			wallColors[19][0] = color;
			color = Color.FromArgb(72, 50, 77);
			wallColors[96][0] = color;
			wallColors[102][0] = color;
			color = Color.FromArgb(78, 50, 69);
			wallColors[97][0] = color;
			wallColors[103][0] = color;
			wallColors[10][0] = Color.FromArgb(74, 62, 12);
			wallColors[11][0] = Color.FromArgb(46, 56, 59);
			wallColors[12][0] = Color.FromArgb(75, 32, 11);
			wallColors[13][0] = Color.FromArgb(67, 37, 37);
			color = Color.FromArgb(15, 15, 15);
			wallColors[14][0] = color;
			wallColors[20][0] = color;
			wallColors[15][0] = Color.FromArgb(52, 43, 45);
			wallColors[22][0] = Color.FromArgb(113, 99, 99);
			wallColors[23][0] = Color.FromArgb(38, 38, 43);
			wallColors[24][0] = Color.FromArgb(53, 39, 41);
			wallColors[25][0] = Color.FromArgb(11, 35, 62);
			wallColors[26][0] = Color.FromArgb(21, 63, 70);
			wallColors[27][0] = Color.FromArgb(88, 61, 46);
			wallColors[27][1] = Color.FromArgb(52, 52, 52);
			wallColors[28][0] = Color.FromArgb(81, 84, 101);
			wallColors[29][0] = Color.FromArgb(88, 23, 23);
			wallColors[30][0] = Color.FromArgb(28, 88, 23);
			wallColors[31][0] = Color.FromArgb(78, 87, 99);
			color = Color.FromArgb(69, 67, 41);
			wallColors[34][0] = color;
			wallColors[37][0] = color;
			wallColors[32][0] = Color.FromArgb(86, 17, 40);
			wallColors[33][0] = Color.FromArgb(49, 47, 83);
			wallColors[35][0] = Color.FromArgb(51, 51, 70);
			wallColors[36][0] = Color.FromArgb(87, 59, 55);
			wallColors[38][0] = Color.FromArgb(49, 57, 49);
			wallColors[39][0] = Color.FromArgb(78, 79, 73);
			wallColors[45][0] = Color.FromArgb(60, 59, 51);
			wallColors[46][0] = Color.FromArgb(48, 57, 47);
			wallColors[47][0] = Color.FromArgb(71, 77, 85);
			wallColors[40][0] = Color.FromArgb(85, 102, 103);
			wallColors[41][0] = Color.FromArgb(52, 50, 62);
			wallColors[42][0] = Color.FromArgb(71, 42, 44);
			wallColors[43][0] = Color.FromArgb(73, 66, 50);
			wallColors[54][0] = Color.FromArgb(40, 56, 50);
			wallColors[55][0] = Color.FromArgb(49, 48, 36);
			wallColors[56][0] = Color.FromArgb(43, 33, 32);
			wallColors[57][0] = Color.FromArgb(31, 40, 49);
			wallColors[58][0] = Color.FromArgb(48, 35, 52);
			wallColors[60][0] = Color.FromArgb(1, 52, 20);
			wallColors[61][0] = Color.FromArgb(55, 39, 26);
			wallColors[62][0] = Color.FromArgb(39, 33, 26);
			wallColors[69][0] = Color.FromArgb(43, 42, 68);
			wallColors[70][0] = Color.FromArgb(30, 70, 80);
			color = Color.FromArgb(30, 80, 48);
			wallColors[63][0] = color;
			wallColors[65][0] = color;
			wallColors[66][0] = color;
			wallColors[68][0] = color;
			color = Color.FromArgb(53, 80, 30);
			wallColors[64][0] = color;
			wallColors[67][0] = color;
			wallColors[78][0] = Color.FromArgb(63, 39, 26);
			wallColors[71][0] = Color.FromArgb(78, 105, 135);
			wallColors[72][0] = Color.FromArgb(52, 84, 12);
			wallColors[73][0] = Color.FromArgb(190, 204, 223);
			color = Color.FromArgb(64, 62, 80);
			wallColors[74][0] = color;
			wallColors[80][0] = color;
			wallColors[75][0] = Color.FromArgb(65, 65, 35);
			wallColors[76][0] = Color.FromArgb(20, 46, 104);
			wallColors[77][0] = Color.FromArgb(61, 13, 16);
			wallColors[79][0] = Color.FromArgb(51, 47, 96);
			wallColors[81][0] = Color.FromArgb(101, 51, 51);
			wallColors[82][0] = Color.FromArgb(77, 64, 34);
			wallColors[83][0] = Color.FromArgb(62, 38, 41);
			wallColors[84][0] = Color.FromArgb(48, 78, 93);
			wallColors[85][0] = Color.FromArgb(54, 63, 69);
			color = Color.FromArgb(138, 73, 38);
			wallColors[86][0] = color;
			wallColors[108][0] = color;
			color = Color.FromArgb(50, 15, 8);
			wallColors[87][0] = color;
			wallColors[112][0] = color;
			wallColors[109][0] = Color.FromArgb(94, 25, 17);
			wallColors[110][0] = Color.FromArgb(125, 36, 122);
			wallColors[111][0] = Color.FromArgb(51, 35, 27);
			wallColors[113][0] = Color.FromArgb(135, 58, 0);
			wallColors[114][0] = Color.FromArgb(65, 52, 15);
			wallColors[115][0] = Color.FromArgb(39, 42, 51);
			wallColors[116][0] = Color.FromArgb(89, 26, 27);
			wallColors[117][0] = Color.FromArgb(126, 123, 115);
			wallColors[118][0] = Color.FromArgb(8, 50, 19);
			wallColors[119][0] = Color.FromArgb(95, 21, 24);
			wallColors[120][0] = Color.FromArgb(17, 31, 65);
			wallColors[121][0] = Color.FromArgb(192, 173, 143);
			wallColors[122][0] = Color.FromArgb(114, 114, 131);
			wallColors[123][0] = Color.FromArgb(136, 119, 7);
			wallColors[124][0] = Color.FromArgb(8, 72, 3);
			wallColors[125][0] = Color.FromArgb(117, 132, 82);
			wallColors[126][0] = Color.FromArgb(100, 102, 114);
			wallColors[127][0] = Color.FromArgb(30, 118, 226);
			wallColors[128][0] = Color.FromArgb(93, 6, 102);
			wallColors[129][0] = Color.FromArgb(64, 40, 169);
			wallColors[130][0] = Color.FromArgb(39, 34, 180);
			wallColors[131][0] = Color.FromArgb(87, 94, 125);
			wallColors[132][0] = Color.FromArgb(6, 6, 6);
			wallColors[133][0] = Color.FromArgb(69, 72, 186);
			wallColors[134][0] = Color.FromArgb(130, 62, 16);
			wallColors[135][0] = Color.FromArgb(22, 123, 163);
			wallColors[136][0] = Color.FromArgb(40, 86, 151);
			wallColors[137][0] = Color.FromArgb(183, 75, 15);
			wallColors[138][0] = Color.FromArgb(83, 80, 100);
			wallColors[139][0] = Color.FromArgb(115, 65, 68);
			wallColors[140][0] = Color.FromArgb(119, 108, 81);
			wallColors[141][0] = Color.FromArgb(59, 67, 71);
			wallColors[142][0] = Color.FromArgb(17, 172, 143);
			wallColors[143][0] = Color.FromArgb(90, 112, 105);
			wallColors[144][0] = Color.FromArgb(62, 28, 87);
			Color[] array4 = new Color[256];
			Color color2 = Color.FromArgb(50, 40, 255);
			Color color3 = Color.FromArgb(145, 185, 255);
			for (int k = 0; k < array4.Length; k++)
			{
				float num = (float)k / (float)array4.Length;
				float num2 = 1f - num;
				array4[k] = Color.FromArgb((int)((byte)((float)color2.R * num2 + (float)color3.R * num)), (int)((byte)((float)color2.G * num2 + (float)color3.G * num)), (int)((byte)((float)color2.B * num2 + (float)color3.B * num)));
			}
			Color[] array5 = new Color[256];
			Color color4 = Color.FromArgb(88, 61, 46);
			Color color5 = Color.FromArgb(37, 78, 123);
			for (int l = 0; l < array5.Length; l++)
			{
				float num3 = (float)l / 255f;
				float num4 = 1f - num3;
				array5[l] = Color.FromArgb((int)((byte)((float)color4.R * num4 + (float)color5.R * num3)), (int)((byte)((float)color4.G * num4 + (float)color5.G * num3)), (int)((byte)((float)color4.B * num4 + (float)color5.B * num3)));
			}
			Color[] array6 = new Color[256];
			Color color6 = Color.FromArgb(74, 67, 60);
			color5 = Color.FromArgb(53, 70, 97);
			for (int m = 0; m < array6.Length; m++)
			{
				float num5 = (float)m / 255f;
				float num6 = 1f - num5;
				array6[m] = Color.FromArgb((int)((byte)((float)color6.R * num6 + (float)color5.R * num5)), (int)((byte)((float)color6.G * num6 + (float)color5.G * num5)), (int)((byte)((float)color6.B * num6 + (float)color5.B * num5)));
			}
			Color color7 = Color.FromArgb(50, 44, 38);
			int num7 = 0;
			Map.tileOptionCounts = new int[314];
			for (int n = 0; n < 314; n++)
			{
				Color[] array7 = tileColors[n];
				int num8 = 0;
				while (num8 < 10 && !(array7[num8] == Color.Transparent))
				{
					num8++;
				}
				Map.tileOptionCounts[n] = num8;
				num7 += num8;
			}
			Map.wallOptionCounts = new int[145];
			for (int num9 = 0; num9 < 145; num9++)
			{
				Color[] array8 = wallColors[num9];
				int num10 = 0;
				while (num10 < 2 && !(array8[num10] == Color.Transparent))
				{
					num10++;
				}
				Map.wallOptionCounts[num9] = num10;
				num7 += num10;
			}
			num7 += 773;
			Map.colorLookup = new Color[num7];
			Map.colorLookup[0] = Color.Transparent;
			ushort num11 = 1;
			Map.tilePosition = num11;
			Map.tileLookup = new ushort[314];
			for (int num12 = 0; num12 < 314; num12++)
			{
				if (Map.tileOptionCounts[num12] > 0)
				{
					Color[] arg_3D44_0 = tileColors[num12];
					Map.tileLookup[num12] = num11;
					for (int num13 = 0; num13 < Map.tileOptionCounts[num12]; num13++)
					{
						Map.colorLookup[(int)num11] = tileColors[num12][num13];
						num11 += 1;
					}
				}
				else
				{
					Map.tileLookup[num12] = 0;
				}
			}
			Map.wallPosition = num11;
			Map.wallLookup = new ushort[145];
			for (int num14 = 0; num14 < 145; num14++)
			{
				if (Map.wallOptionCounts[num14] > 0)
				{
					Color[] arg_3DD2_0 = wallColors[num14];
					Map.wallLookup[num14] = num11;
					for (int num15 = 0; num15 < Map.wallOptionCounts[num14]; num15++)
					{
						Map.colorLookup[(int)num11] = wallColors[num14][num15];
						num11 += 1;
					}
				}
				else
				{
					Map.wallLookup[num14] = 0;
				}
			}
			Map.liquidPosition = num11;
			for (int num16 = 0; num16 < 3; num16++)
			{
				Map.colorLookup[(int)num11] = liquidColors[num16];
				num11 += 1;
			}
			Map.skyPosition = num11;
			for (int num17 = 0; num17 < 256; num17++)
			{
				Map.colorLookup[(int)num11] = array4[num17];
				num11 += 1;
			}
			Map.dirtPosition = num11;
			for (int num18 = 0; num18 < 256; num18++)
			{
				Map.colorLookup[(int)num11] = array5[num18];
				num11 += 1;
			}
			Map.rockPosition = num11;
			for (int num19 = 0; num19 < 256; num19++)
			{
				Map.colorLookup[(int)num11] = array6[num19];
				num11 += 1;
			}
			Map.hellPosition = num11;
			Map.colorLookup[(int)num11] = color7;
			Map.snowTypes = new ushort[6];
			Map.snowTypes[0] = Map.tileLookup[147];
			Map.snowTypes[1] = Map.tileLookup[161];
			Map.snowTypes[2] = Map.tileLookup[162];
			Map.snowTypes[3] = Map.tileLookup[163];
			Map.snowTypes[4] = Map.tileLookup[164];
			Map.snowTypes[5] = Map.tileLookup[200];
		}

		public static System.Drawing.Color GetTileColor(ushort tileType, short u, short v)
		{
			Color color = tileColors[tileType][0];

			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Color GetWallColor(ushort wallType)
		{
			Color color = wallColors[wallType][0];

			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Color GetLiquidColor(ushort liquidType)
		{
			Color color = liquidColors[liquidType];

			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
