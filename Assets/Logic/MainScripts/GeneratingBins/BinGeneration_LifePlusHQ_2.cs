using UnityEngine;
using System.Collections;
using System.IO;

public class BinGeneration_LifePlusHQ_2 : MonoBehaviour {
	string path;

	public void Init() {
		path = COMMON.dataFolder + "LifePlusHQ/";
		Narration(); Other(); Chats();
		Debug.Log("LifePlusHQ-2 bins generated.");
		GetComponent<DataControlHub>().Init();
	}

	void Narration() {
		var NS = new NarrationStructure(); int size = 25;
		NS.Next = new int[size]; NS.TriggersEvent = new int?[size];
		NS.Finisher = new bool[size]; NS.Additive = new int[size];
		//news
		NS.Next[0] = 1; NS.Next[1] = 2; NS.Next[2] = 3; NS.Next[3] = 4;
		NS.Additive[1] = 1; NS.Additive[3] = 1; NS.Additive[4] = 1;
		NS.Finisher[4] = true; NS.Finisher[5] = true;
		//chapter 1
		NS.Next[10] = 11; NS.Next[11] = 13; NS.Next[12] = 13; NS.Finisher[13] = true;
		NS.Additive[11] = 1; NS.Additive[12] = 1; NS.Additive[13] = 1;
		NS.TriggersEvent[11] = (int)eventsH.blackScreenOff;
		NS.TriggersEvent[12] = (int)eventsH.blackScreenOff;
		NS.TriggersEvent[13] = (int)eventsH.startDialogue1;
		//chapter 1 - chill
		NS.Next[20] = 21; NS.Next[21] = 22; NS.Next[22] = 23; NS.Finisher[23] = true;
		NS.Additive[21] = 1; NS.Additive[23] = 1;
		NS.TriggersEvent[22] = (int)eventsH.blackScreenOff;
		NS.TriggersEvent[23] = (int)eventsH.firstNightStarted;
		NS.Save(path + "Narration.bin");
	}
	
	void Other() {
		var DS = new DialogueStructure(); int size = 72;
		DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];
		
		DS.R[0] = new int[] {1, 2};
		DS.R[2] = new int[] {0};
		DS.R[10] = new int[] {12, 13, 14, 15};
		DS.R[11] = new int[] {12, 13, 14, 15};
		DS.R[12] = new int[] {11};
		DS.R[13] = new int[] {11};
		DS.R[14] = new int[] {11};
		DS.R[15] = new int[] {11};
		DS.R[20] = new int[] {21, 22};
		DS.R[21] = new int[] {40};
		DS.R[22] = new int[] {23};
		DS.R[23] = new int[] {24, 25};
		DS.R[24] = new int[] {40};
		DS.R[25] = new int[] {26};
		DS.R[26] = new int[] {27, 28};
		DS.R[27] = new int[] {40};
		DS.R[28] = new int[] {29};
		DS.R[29] = new int[] {30, 31};
		DS.R[30] = new int[] {40};
		DS.R[31] = new int[] {32};
		DS.R[32] = new int[] {33, 34};
		DS.R[33] = new int[] {40};
		DS.R[34] = new int[] {35};
		DS.R[35] = new int[] {36, 37};
		DS.R[36] = new int[] {40};
		DS.R[37] = new int[] {20};
		DS.R[40] = new int[] {41, 42};
		DS.R[41] = new int[] {60};
		DS.R[42] = new int[] {43};
		DS.R[43] = new int[] {44, 45};
		DS.R[44] = new int[] {60};
		DS.R[45] = new int[] {46};
		DS.R[46] = new int[] {47, 48};
		DS.R[47] = new int[] {60};
		DS.R[48] = new int[] {49};
		DS.R[49] = new int[] {50, 51};
		DS.R[50] = new int[] {60};
		DS.R[51] = new int[] {52};
		DS.R[52] = new int[] {53, 54};
		DS.R[53] = new int[] {60};
		DS.R[54] = new int[] {55};
		DS.R[55] = new int[] {56, 57};
		DS.R[56] = new int[] {60};
		DS.R[57] = new int[] {40};
		DS.R[60] = new int[] {61, 62};
		DS.R[61] = null;
		DS.R[62] = new int[] {63};
		DS.R[63] = new int[] {64, 65};
		DS.R[64] = null;
		DS.R[65] = new int[] {66};
		DS.R[66] = new int[] {67, 68};
		DS.R[67] = null;
		DS.R[68] = new int[] {69};
		DS.R[69] = new int[] {70, 71};
		DS.R[70] = null;
		DS.R[71] = new int[] {60};
		
		DS.Finisher[1] = true; DS.Finisher[2] = true;
		DS.Finisher[12] = true; DS.Finisher[13] = true; DS.Finisher[14] = true; DS.Finisher[15] = true;
		DS.Finisher[21] = true; DS.Finisher[22] = true; DS.Finisher[24] = true; DS.Finisher[25] = true;
		DS.Finisher[27] = true; DS.Finisher[28] = true; DS.Finisher[30] = true; DS.Finisher[31] = true;
		DS.Finisher[33] = true; DS.Finisher[34] = true; DS.Finisher[36] = true; DS.Finisher[37] = true;
		DS.Finisher[41] = true; DS.Finisher[42] = true; DS.Finisher[44] = true; DS.Finisher[45] = true;
		DS.Finisher[47] = true; DS.Finisher[48] = true; DS.Finisher[50] = true; DS.Finisher[51] = true;
		DS.Finisher[53] = true; DS.Finisher[54] = true; DS.Finisher[56] = true; DS.Finisher[57] = true;
		DS.Finisher[61] = true; DS.Finisher[62] = true; DS.Finisher[64] = true; DS.Finisher[65] = true;
		DS.Finisher[67] = true; DS.Finisher[68] = true; DS.Finisher[70] = true; DS.Finisher[71] = true;
		DS.TriggersEvent[1] = (int)eventsH.firstTransitionToLobby;
		DS.TriggersEvent[12] = (int)eventsH.oyasumi;
		DS.TriggersEvent[21] = (int)eventsH.oyasumi; DS.TriggersEvent[24] = (int)eventsH.oyasumi;
		DS.TriggersEvent[27] = (int)eventsH.oyasumi; DS.TriggersEvent[30] = (int)eventsH.oyasumi;
		DS.TriggersEvent[33] = (int)eventsH.oyasumi; DS.TriggersEvent[36] = (int)eventsH.oyasumi;
		DS.TriggersEvent[41] = (int)eventsH.goToMemories; DS.TriggersEvent[44] = (int)eventsH.goToMemories;
		DS.TriggersEvent[47] = (int)eventsH.goToMemories; DS.TriggersEvent[50] = (int)eventsH.goToMemories;
		DS.TriggersEvent[53] = (int)eventsH.goToMemories; DS.TriggersEvent[56] = (int)eventsH.goToMemories;
		DS.TriggersEvent[61] = (int)eventsH.oyasumi; DS.TriggersEvent[64] = (int)eventsH.oyasumi;
		DS.TriggersEvent[67] = (int)eventsH.oyasumi; DS.TriggersEvent[70] = (int)eventsH.oyasumi;
		
		DS.Save(path + "Other.bin");
	}
	
	void Chats() {
		var DS = new DialogueStructure(); int size = 651;
		DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		//INTROS
		DS.R[0] = new int[] {1, 2, 3, 4};
		DS.R[1] = new int[] {5};
		DS.R[2] = new int[] {5};
		DS.R[3] = new int[] {5};
		DS.R[4] = new int[] {5};
		DS.R[5] = new int[] {6, 7, 8};
		DS.R[6] = new int[] {9};
		DS.R[7] = new int[] {10};
		DS.R[8] = new int[] {11};
		DS.R[9] = new int[] {55, 85, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[10] = new int[] {55, 85, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[11] = new int[] {12};
		DS.R[12] = new int[] {6, 7, 13};
		DS.R[13] = new int[] {11};
		DS.R[15] = new int[] {16, 17, 18};
		DS.R[16] = new int[] {19};
		DS.R[17] = new int[] {20};
		DS.R[18] = new int[] {21};
		DS.R[19] = new int[] {6, 7, 8};
		DS.R[20] = new int[] {6, 7, 8};
		DS.R[21] = new int[] {6, 7, 8};
		DS.R[23] = new int[] {6, 7, 13};
		DS.R[24] = new int[] {25, 26, 27};
		DS.R[25] = new int[] {28};
		DS.R[26] = new int[] {29};
		DS.R[27] = new int[] {52};
		DS.R[28] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[29] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[30] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 50}; //!!!
		DS.R[32] = new int[] {33, 34};
		DS.R[33] = new int[] {35};
		DS.R[34] = new int[] {36};
		DS.R[35] = new int[] {6, 7, 8};
		DS.R[36] = new int[] {6, 7, 8};
		DS.R[37] = new int[] {55, 85, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[40] = new int[] {41, 42};
		DS.R[41] = new int[] {43};
		DS.R[42] = new int[] {44};
		DS.R[43] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 50}; //!!!
		DS.R[44] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 50}; //!!!
		DS.R[45] = new int[] {46};
		DS.R[46] = new int[] {47, 48};
		DS.R[47] = null;
		DS.R[48] = null;
		DS.R[50] = new int[] {52};
		DS.R[51] = new int[] {52};
		DS.R[52] = new int[] {53};
		DS.R[53] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 50}; //!!!

		//1A. NI HAO
		DS.R[55] = new int[] {56};
		DS.R[56] = new int[] {57, 58};
		DS.R[57] = new int[] {59};
		DS.R[58] = new int[] {59};
		DS.R[59] = new int[] {60, 61, 62};
		DS.R[60] = new int[] {63};
		DS.R[61] = new int[] {63};
		DS.R[62] = new int[] {63};
		DS.R[63] = new int[] {64, 65};
		DS.R[64] = new int[] {66};
		DS.R[65] = new int[] {67};
		DS.R[66] = new int[] {68, 70, 71, 72};
		DS.R[67] = new int[] {68, 70, 71, 72};
		DS.R[68] = new int[] {73};
		DS.R[69] = new int[] {74};
		DS.R[70] = new int[] {75};
		DS.R[71] = new int[] {76};
		DS.R[72] = new int[] {77};
		DS.R[73] = new int[] {69, 70, 71, 72};
		DS.R[74] = new int[] {70, 71, 72};
		DS.R[75] = new int[] {68, 69, 71, 72};
		DS.R[76] = new int[] {68, 69, 70, 72};
		DS.R[77] = new int[] {85, 126, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!

		//1B. FEELS LIKE FUTURE
		DS.R[85] = new int[] {86};
		DS.R[86] = new int[] {87, 88};
		DS.R[87] = new int[] {89};
		DS.R[88] = new int[] {90};
		DS.R[89] = new int[] {91, 92, 93};
		DS.R[90] = new int[] {91, 92, 93};
		DS.R[91] = new int[] {94};
		DS.R[92] = new int[] {95};
		DS.R[93] = new int[] {96};
		DS.R[94] = new int[] {98, 99, 97, 100};
		DS.R[95] = new int[] {98, 99, 97, 100};
		DS.R[96] = new int[] {98, 99, 97, 100};
		DS.R[97] = new int[] {104};
		DS.R[98] = new int[] {102};
		DS.R[99] = new int[] {103};
		DS.R[100] = new int[] {101};
		DS.R[101] = new int[] {55, 126, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[102] = new int[] {97, 100};
		DS.R[103] = new int[] {97, 100};
		DS.R[104] = new int[] {105, 106};
		DS.R[105] = new int[] {107};
		DS.R[106] = new int[] {107};
		DS.R[107] = new int[] {108, 109, 110, 111, 112, 113};
		DS.R[108] = new int[] {114};
		DS.R[109] = new int[] {115};
		DS.R[110] = new int[] {116};
		DS.R[111] = new int[] {117};
		DS.R[112] = new int[] {118};
		DS.R[113] = new int[] {125};
		DS.R[114] = new int[] {109, 110, 111, 112, 113};
		DS.R[115] = new int[] {108, 110, 111, 112, 113};
		DS.R[116] = new int[] {108, 109, 111, 112, 113};
		DS.R[117] = new int[] {108, 109, 110, 112, 113};
		DS.R[118] = new int[] {119, 120, 121};
		DS.R[119] = new int[] {122};
		DS.R[120] = new int[] {123};
		DS.R[121] = new int[] {124};
		DS.R[122] = new int[] {108, 109, 110, 111, 113};
		DS.R[123] = new int[] {108, 109, 110, 111, 113};
		DS.R[124] = new int[] {108, 109, 110, 111, 113};
		DS.R[125] = new int[] {55, 170, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[126] = new int[] {104};

		//1Extra. BOSS
		DS.R[135] = new int[] {136};
		DS.R[136] = new int[] {137};
		DS.R[137] = new int[] {138};
		DS.R[138] = new int[] {139, 140};
		DS.R[139] = new int[] {141};
		DS.R[140] = new int[] {142};
		DS.R[141] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 150, 51}; //!!!
		DS.R[142] = new int[] {143, 144};
		DS.R[143] = new int[] {145};
		DS.R[144] = new int[] {146};
		DS.R[145] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 150, 51}; //!!!
		DS.R[146] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 150, 51}; //!!!
		//2Extra.LOBBY
		DS.R[150] = new int[] {151};
		DS.R[151] = new int[] {152, 154, 155};
		DS.R[152] = new int[] {156};
		DS.R[153] = new int[] {157};
		DS.R[154] = new int[] {158};
		DS.R[155] = new int[] {159};
		DS.R[156] = new int[] {153, 154, 155};
		DS.R[157] = new int[] {154, 155};
		DS.R[158] = new int[] {152, 153, 154, 155};
		DS.R[159] = new int[] {55, 85, 126, 170, 244, 350, 351, 520, 521, 135, 51}; //!!!

		//2A. NEEDED A REMINDER
		DS.R[170] = new int[] {171};
		DS.R[171] = new int[] {172, 173, 175, 176, 177};
		DS.R[172] = new int[] {178};
		DS.R[173] = new int[] {179};
		DS.R[174] = new int[] {179};
		DS.R[175] = new int[] {180};
		DS.R[176] = new int[] {181};
		DS.R[177] = new int[] {182};
		DS.R[178] = new int[] {174, 175, 176, 177};
		DS.R[179] = new int[] {183};
		DS.R[180] = new int[] {183};
		DS.R[181] = new int[] {183};
		DS.R[182] = new int[] {183};
		DS.R[183] = new int[] {184};
		DS.R[184] = new int[] {185, 186, 187};
		DS.R[185] = new int[] {188};
		DS.R[186] = new int[] {189};
		DS.R[187] = new int[] {190};
		DS.R[188] = new int[] {191, 192, 193, 194};
		DS.R[189] = new int[] {191, 192, 193, 194};
		DS.R[190] = new int[] {191, 192, 193, 194};
		DS.R[191] = new int[] {195};
		DS.R[192] = new int[] {196};
		DS.R[193] = new int[] {207};
		DS.R[194] = new int[] {208};
		DS.R[195] = new int[] {192, 193, 194};
		DS.R[196] = new int[] {197, 198};
		DS.R[197] = new int[] {199};
		DS.R[198] = new int[] {200};
		DS.R[199] = new int[] {201, 202, 203};
		DS.R[200] = new int[] {201, 202, 203};
		DS.R[201] = new int[] {204};
		DS.R[202] = new int[] {205};
		DS.R[203] = new int[] {206};
		DS.R[204] = new int[] {191, 193, 194};
		DS.R[205] = new int[] {191, 193, 194};
		DS.R[206] = new int[] {191, 193, 194};
		DS.R[207] = new int[] {191, 192, 194};
		DS.R[208] = new int[] {209};
		DS.R[209] = new int[] {210};
		DS.R[210] = new int[] {211, 212, 213};
		DS.R[211] = new int[] {214};
		DS.R[212] = new int[] {215};
		DS.R[213] = new int[] {216};
		DS.R[214] = new int[] {212, 213};
		DS.R[215] = new int[] {211, 213};
		DS.R[216] = new int[] {217, 218, 219};
		DS.R[217] = new int[] {220};
		DS.R[218] = new int[] {221};
		DS.R[219] = new int[] {222};
		DS.R[220] = new int[] {223, 224, 225};
		DS.R[221] = new int[] {223, 224, 225};
		DS.R[222] = new int[] {223, 224, 225};
		DS.R[223] = new int[] {226};
		DS.R[224] = new int[] {232};
		DS.R[225] = new int[] {235};
		DS.R[226] = new int[] {55, 85, 126, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[227] = new int[] {228, 229};
		DS.R[228] = new int[] {230};
		DS.R[229] = new int[] {231};
		DS.R[230] = new int[] {55, 85, 126, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[231] = new int[] {55, 85, 126, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[232] = new int[] {233};
		DS.R[233] = new int[] {234};
		DS.R[234] = new int[] {55, 85, 126, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!
		DS.R[235] = new int[] {55, 85, 126, 244, 350, 351, 520, 521, 135, 150, 51}; //!!!

		//2B. TRUE STORY
		DS.R[244] = new int[] {245};
		DS.R[245] = new int[] {246, 247, 248};
		DS.R[246] = new int[] {249};
		DS.R[247] = new int[] {260};
		DS.R[248] = new int[] {275};
		DS.R[249] = new int[] {250, 251};
		DS.R[250] = new int[] {252};
		DS.R[251] = new int[] {253};
		DS.R[252] = new int[] {254};
		DS.R[253] = new int[] {254};
		DS.R[254] = new int[] {255};
		DS.R[255] = new int[] {256};
		DS.R[256] = new int[] {258};
		DS.R[257] = new int[] {259};
		DS.R[258] = new int[] {259};
		DS.R[259] = new int[] {290};
		DS.R[260] = new int[] {261};
		DS.R[261] = new int[] {262};
		DS.R[262] = new int[] {263, 264, 265};
		DS.R[263] = new int[] {268};
		DS.R[264] = new int[] {269};
		DS.R[265] = new int[] {266};
		DS.R[266] = new int[] {267};
		DS.R[267] = new int[] {255};
		DS.R[268] = new int[] {270};
		DS.R[269] = new int[] {271};
		DS.R[270] = new int[] {273};
		DS.R[271] = new int[] {273};
		DS.R[272] = new int[] {274};
		DS.R[273] = new int[] {274};
		DS.R[274] = new int[] {290};
		DS.R[275] = new int[] {276};
		DS.R[276] = new int[] {277};
		DS.R[277] = new int[] {278};
		DS.R[278] = new int[] {279};
		DS.R[279] = new int[] {281};
		DS.R[280] = new int[] {282};
		DS.R[281] = new int[] {282};
		DS.R[282] = new int[] {283};
		DS.R[283] = new int[] {290};
		DS.R[290] = new int[] {291, 292};
		DS.R[291] = new int[] {293};
		DS.R[292] = new int[] {294};
		DS.R[293] = new int[] {295, 296, 298, 299};
		DS.R[294] = new int[] {295, 296, 298, 299};
		DS.R[295] = new int[] {300};
		DS.R[296] = new int[] {306};
		DS.R[297] = new int[] {311};
		DS.R[298] = new int[] {312};
		DS.R[299] = new int[] {313};
		DS.R[300] = new int[] {301, 302, 303};
		DS.R[301] = new int[] {305};
		DS.R[302] = new int[] {305};
		DS.R[303] = new int[] {304};
		DS.R[304] = new int[] {296, 297, 298, 299};
		DS.R[305] = new int[] {296, 297, 298, 299};
		DS.R[306] = new int[] {307};
		DS.R[307] = new int[] {309};
		DS.R[308] = new int[] {310};
		DS.R[309] = new int[] {295, 297, 298, 299};
		DS.R[310] = new int[] {295, 297, 298, 299};
		DS.R[311] = new int[] {295, 298, 299};
		DS.R[312] = new int[] {295, 296, 297, 299};
		DS.R[313] = new int[] {314, 315};
		DS.R[314] = new int[] {316};
		DS.R[315] = new int[] {317};
		DS.R[316] = new int[] {315};
		DS.R[317] = new int[] {318};
		DS.R[318] = new int[] {319};
		DS.R[319] = new int[] {320, 322, 324};
		DS.R[320] = new int[] {325};
		DS.R[321] = new int[] {336};
		DS.R[322] = new int[] {337};
		DS.R[323] = new int[] {337};
		DS.R[324] = new int[] {338};
		DS.R[325] = new int[] {326, 327};
		DS.R[326] = new int[] {328};
		DS.R[327] = new int[] {329};
		DS.R[328] = new int[] {330, 331, 332};
		DS.R[329] = new int[] {330, 331, 332};
		DS.R[330] = new int[] {333};
		DS.R[331] = new int[] {334};
		DS.R[332] = new int[] {335};
		DS.R[333] = new int[] {321, 323, 324};
		DS.R[334] = new int[] {321, 323, 324};
		DS.R[335] = new int[] {321, 323, 324};
		DS.R[336] = new int[] {323, 324};
		DS.R[337] = new int[] {320, 321, 323, 324};
		DS.R[338] = new int[] {55, 85, 126, 170, 350, 351, 520, 521, 135, 150, 51}; //!!!

		//3A. PERSONAL STUFF
		DS.R[350] = new int[] {352};
		DS.R[351] = new int[] {359};
		DS.R[352] = new int[] {353, 354, 355};
		DS.R[353] = new int[] {356};
		DS.R[354] = new int[] {357};
		DS.R[355] = new int[] {358};
		DS.R[356] = new int[] {360, 361, 362, 363, 364, 365};
		DS.R[357] = new int[] {360, 361, 362, 363, 364, 365};
		DS.R[358] = new int[] {360, 361, 362, 363, 364, 365};
		DS.R[359] = new int[] {360, 361, 362, 363, 364, 365};
		DS.R[360] = new int[] {370};
		DS.R[361] = new int[] {395};
		DS.R[362] = new int[] {440};
		DS.R[363] = new int[] {460};
		DS.R[364] = new int[] {475};
		DS.R[365] = new int[] {366};
		DS.R[366] = new int[] {55, 85, 126, 170, 244, 351, 520, 521, 135, 150, 51}; //!!!
		//1
		DS.R[370] = new int[] {371, 372, 373};
		DS.R[371] = new int[] {374};
		DS.R[372] = new int[] {378};
		DS.R[373] = new int[] {379};
		DS.R[374] = new int[] {375, 376, 377};
		DS.R[375] = new int[] {382};
		DS.R[376] = new int[] {383};
		DS.R[377] = new int[] {384};
		DS.R[378] = new int[] {380, 381};
		DS.R[379] = new int[] {380, 381};
		DS.R[380] = new int[] {385};
		DS.R[381] = new int[] {385};
		DS.R[382] = new int[] {386, 388, 389};
		DS.R[383] = new int[] {386, 388, 389};
		DS.R[384] = new int[] {386, 388, 389};
		DS.R[385] = new int[] {386, 388, 389};
		DS.R[386] = new int[] {390};
		DS.R[387] = new int[] {393};
		DS.R[388] = new int[] {394};
		DS.R[389] = new int[] {369};
		DS.R[390] = new int[] {391};
		DS.R[391] = new int[] {392};
		DS.R[392] = new int[] {387, 388, 389};
		DS.R[393] = new int[] {388, 389};
		DS.R[394] = new int[] {386, 387, 389};
		DS.R[369] = new int[] {361, 362, 363, 364, 365};
		//2
		DS.R[395] = new int[] {396, 397};
		DS.R[396] = new int[] {398};
		DS.R[397] = new int[] {399};
		DS.R[398] = new int[] {400};
		DS.R[399] = new int[] {400};
		DS.R[400] = new int[] {401};
		DS.R[401] = new int[] {402};
		DS.R[402] = new int[] {403};
		DS.R[403] = new int[] {404};
		DS.R[404] = new int[] {405};
		DS.R[405] = new int[] {408};
		DS.R[408] = new int[] {409};
		DS.R[409] = new int[] {410, 411, 412, 413};
		DS.R[410] = new int[] {414};
		DS.R[411] = new int[] {415};
		DS.R[412] = new int[] {416};
		DS.R[413] = new int[] {417};
		DS.R[414] = new int[] {411, 412, 413};
		DS.R[415] = new int[] {410, 412, 413};
		DS.R[416] = new int[] {410, 411, 413};
		DS.R[417] = new int[] {418, 419};
		DS.R[418] = new int[] {420};
		DS.R[419] = new int[] {421};
		DS.R[420] = new int[] {422};
		DS.R[421] = new int[] {422, 423};
		DS.R[422] = new int[] {424};
		DS.R[423] = new int[] {427};
		DS.R[424] = new int[] {425};
		DS.R[425] = new int[] {426};
		DS.R[426] = new int[] {360, 362, 363, 364, 365};
		DS.R[427] = new int[] {428, 429};
		DS.R[428] = new int[] {430};
		DS.R[429] = new int[] {431};
		DS.R[430] = new int[] {360, 362, 363, 364, 365};
		DS.R[431] = new int[] {360, 362, 363, 364, 365};
		//3
		DS.R[440] = new int[] {441, 442};
		DS.R[441] = new int[] {443};
		DS.R[442] = new int[] {444};
		DS.R[443] = new int[] {445, 446, 447, 448, 449};
		DS.R[444] = new int[] {445, 446, 447, 448, 449};
		DS.R[445] = new int[] {451};
		DS.R[446] = new int[] {452};
		DS.R[447] = new int[] {453};
		DS.R[448] = new int[] {454};
		DS.R[449] = new int[] {450};
		DS.R[450] = new int[] {360, 361, 363, 364, 365};
		DS.R[451] = new int[] {446, 447, 448, 449};
		DS.R[452] = new int[] {445, 447, 448, 449};
		DS.R[453] = new int[] {445, 446, 448, 449};
		DS.R[454] = new int[] {445, 446, 447, 449};
		//4
		DS.R[460] = new int[] {461};
		DS.R[461] = new int[] {462};
		DS.R[462] = new int[] {463, 464, 465, 466, 467};
		DS.R[463] = new int[] {468};
		DS.R[464] = new int[] {469};
		DS.R[465] = new int[] {470};
		DS.R[466] = new int[] {471};
		DS.R[467] = new int[] {472};
		DS.R[468] = new int[] {464, 465, 466, 467};
		DS.R[469] = new int[] {463, 465, 466, 467};
		DS.R[470] = new int[] {463, 464, 466, 467};
		DS.R[471] = new int[] {463, 464, 465, 467};
		DS.R[472] = new int[] {360, 361, 362, 364, 365};
		//5
		DS.R[475] = new int[] {476};
		DS.R[476] = new int[] {477};
		DS.R[477] = new int[] {478};
		DS.R[478] = new int[] {479};
		DS.R[479] = new int[] {480};
		DS.R[480] = new int[] {481};
		DS.R[481] = new int[] {482};
		DS.R[482] = new int[] {483};
		DS.R[483] = new int[] {484};
		DS.R[484] = new int[] {485};
		DS.R[485] = new int[] {486, 487, 488};
		DS.R[486] = new int[] {489};
		DS.R[487] = new int[] {490};
		DS.R[488] = new int[] {490};
		DS.R[489] = new int[] {495};
		DS.R[490] = new int[] {491, 492};
		DS.R[491] = new int[] {493};
		DS.R[492] = new int[] {494};
		DS.R[493] = new int[] {495};
		DS.R[494] = new int[] {495};
		DS.R[495] = new int[] {496};
		DS.R[496] = new int[] {497, 498, 499, 500};
		DS.R[497] = new int[] {501};
		DS.R[498] = new int[] {502};
		DS.R[499] = new int[] {503};
		DS.R[500] = new int[] {504};
		DS.R[501] = new int[] {360, 361, 362, 363, 365};
		DS.R[502] = new int[] {360, 361, 362, 363, 365};
		DS.R[503] = new int[] {360, 361, 362, 363, 365};
		DS.R[504] = new int[] {360, 361, 362, 363, 365};

		//3B. CORPORATE STUFF
		DS.R[520] = new int[] {522};
		DS.R[521] = new int[] {522};
		DS.R[522] = new int[] {525, 526, 527, 528, 529, 530, 531};
		DS.R[525] = new int[] {540};
		DS.R[526] = new int[] {550};
		DS.R[527] = new int[] {580};
		DS.R[528] = new int[] {605};
		DS.R[529] = new int[] {635};
		DS.R[530] = new int[] {640};
		DS.R[531] = new int[] {532};
		DS.R[532] = new int[] {55, 85, 126, 170, 244, 350, 351, 521, 135, 150, 51}; //!!!
		//1
		DS.R[540] = new int[] {541, 542, 543};
		DS.R[541] = new int[] {544};
		DS.R[542] = new int[] {545};
		DS.R[543] = new int[] {546};
		DS.R[544] = new int[] {542, 543};
		DS.R[545] = new int[] {541, 543};
		DS.R[546] = new int[] {547};
		DS.R[547] = new int[] {548};
		DS.R[548] = new int[] {526, 527, 528, 529, 530, 531};
		//2
		DS.R[550] = new int[] {551};
		DS.R[551] = new int[] {552};
		DS.R[552] = new int[] {553, 554, 555, 556};
		DS.R[553] = new int[] {557};
		DS.R[554] = new int[] {558};
		DS.R[555] = new int[] {559};
		DS.R[556] = new int[] {560};
		DS.R[557] = new int[] {561, 562, 563};
		DS.R[558] = new int[] {561, 562, 563};
		DS.R[559] = new int[] {561, 562, 563};
		DS.R[560] = new int[] {561, 562, 563};
		DS.R[561] = new int[] {564};
		DS.R[562] = new int[] {565};
		DS.R[563] = new int[] {566};
		DS.R[564] = new int[] {562, 563};
		DS.R[565] = new int[] {561, 563};
		DS.R[566] = new int[] {567, 568, 569, 570, 571};
		DS.R[567] = new int[] {572};
		DS.R[568] = new int[] {573};
		DS.R[569] = new int[] {574};
		DS.R[570] = new int[] {575};
		DS.R[571] = new int[] {576};
		DS.R[572] = new int[] {568, 569, 570, 571};
		DS.R[573] = new int[] {567, 569, 570, 571};
		DS.R[574] = new int[] {567, 568, 570, 571};
		DS.R[575] = new int[] {567, 568, 569, 571};
		DS.R[576] = new int[] {525, 527, 528, 529, 530, 531};
		//3
		DS.R[580] = new int[] {581, 583, 585};
		DS.R[581] = new int[] {586};
		DS.R[582] = new int[] {587};
		DS.R[583] = new int[] {592};
		DS.R[584] = new int[] {599};
		DS.R[585] = new int[] {600};
		DS.R[586] = new int[] {582, 583, 584, 585};
		DS.R[587] = new int[] {588, 589};
		DS.R[588] = new int[] {590};
		DS.R[589] = new int[] {591};
		DS.R[590] = new int[] {583, 584, 585};
		DS.R[591] = new int[] {583, 584, 585};
		DS.R[592] = new int[] {593, 594};
		DS.R[593] = new int[] {595};
		DS.R[594] = new int[] {596};
		DS.R[595] = new int[] {597};
		DS.R[596] = new int[] {597};
		DS.R[597] = new int[] {598};
		DS.R[598] = new int[] {581, 582, 584, 585};
		DS.R[599] = new int[] {581, 582, 585};
		DS.R[600] = new int[] {525, 526, 528, 529, 530, 531};
		//4
		DS.R[605] = new int[] {606, 607, 608};
		DS.R[606] = new int[] {609};
		DS.R[607] = new int[] {610};
		DS.R[608] = new int[] {611};
		DS.R[609] = new int[] {612};
		DS.R[610] = new int[] {612};
		DS.R[611] = new int[] {612};
		DS.R[612] = new int[] {613};
		DS.R[613] = new int[] {614, 615, 616};
		DS.R[614] = new int[] {617};
		DS.R[615] = new int[] {618};
		DS.R[616] = new int[] {619};
		DS.R[617] = new int[] {620, 621};
		DS.R[618] = new int[] {620, 621};
		DS.R[619] = new int[] {620, 621};
		DS.R[620] = new int[] {622};
		DS.R[621] = new int[] {623};
		DS.R[622] = new int[] {624, 625, 626, 627, 628};
		DS.R[623] = new int[] {624, 625, 626, 627, 628};
		DS.R[624] = new int[] {629};
		DS.R[625] = new int[] {630};
		DS.R[626] = new int[] {631};
		DS.R[627] = new int[] {632};
		DS.R[628] = new int[] {633};
		DS.R[629] = new int[] {625, 626, 627, 628};
		DS.R[630] = new int[] {624, 626, 627, 628};
		DS.R[631] = new int[] {624, 625, 627, 628};
		DS.R[632] = new int[] {624, 625, 626, 628};
		DS.R[633] = new int[] {525, 526, 527, 529, 530, 531};
		//5
		DS.R[635] = new int[] {636, 637};
		DS.R[636] = new int[] {638};
		DS.R[637] = new int[] {639};
		DS.R[638] = new int[] {525, 526, 527, 528, 530, 531};
		DS.R[639] = new int[] {525, 526, 527, 528, 530, 531};
		//6
		DS.R[640] = new int[] {641, 642};
		DS.R[641] = new int[] {643};
		DS.R[642] = new int[] {644};
		DS.R[643] = new int[] {645, 646, 647};
		DS.R[644] = new int[] {645, 646, 647};
		DS.R[645] = new int[] {648};
		DS.R[646] = new int[] {649};
		DS.R[647] = new int[] {650};
		DS.R[648] = new int[] {525, 526, 527, 528, 529, 531};
		DS.R[649] = new int[] {525, 526, 527, 528, 529, 531};
		DS.R[650] = new int[] {525, 526, 527, 528, 529, 531};

		DS.Locked[4] = true; //unlocked if hasn't visited on day 1
		DS.Locked[172] = true; //unlocked if was a bit of an ass during prologue
		DS.Locked[212] = true; //unlocked after question 1B
		DS.Locked[375] = true; //unlocked if read SotD1
		DS.Locked[497] = true; //unlocked if talked about skirts
		DS.Locked[150] = true; DS.Locked[170] = true; DS.Locked[244] = true; //unlocked on day 2
		DS.Locked[350] = true; DS.Locked[520] = true; //unlocked on day 3
		DS.Locked[69] = true; DS.UnlockingID[68] = new int[] {69};
		DS.Locked[126] = true; DS.UnlockingID[100] = new int[] {126};
		DS.Locked[153] = true; DS.UnlockingID[152] = new int[] {153};
		DS.Locked[297] = true; DS.UnlockingID[296] = new int[] {297};
		DS.Locked[321] = true; DS.UnlockingID[320] = new int[] {321}; DS.LockingID[322] = new int[] {323};
		DS.Locked[351] = true; DS.UnlockingID[350] = new int[] {351};
		DS.Locked[387] = true; DS.UnlockingID[386] = new int[] {387};
		DS.Locked[521] = true; DS.UnlockingID[520] = new int[] {521};
		DS.Locked[582] = true; DS.UnlockingID[581] = new int[] {582};
		DS.Locked[584] = true; DS.UnlockingID[583] = new int[] {584};

		for (int i = 1; i <= 4; i++) DS.TriggersEvent[i] = (int)eventsH.visitedJackieAtNight;
		for (int i = 16; i <= 18; i++) DS.TriggersEvent[i] = (int)eventsH.visitedJackieAtNight;
		DS.TriggersEvent[33] = (int)eventsH.visitedJackieAtNight; DS.TriggersEvent[34] = (int)eventsH.visitedJackieAtNight;
		DS.TriggersEvent[6] = (int)eventsH.startedQuestions; DS.TriggersEvent[7] = (int)eventsH.startedQuestions;
		DS.TriggersEvent[41] = (int)eventsH.JackieMaybeTurnsB; DS.TriggersEvent[42] = (int)eventsH.JackieMaybeTurnsB;
		DS.TriggersEvent[47] = (int)eventsH.letsStare; DS.TriggersEvent[48] = (int)eventsH.finishedInteraction;
		DS.TriggersEvent[192] = (int)eventsH.annoyingSkirts;
		DS.TriggersEvent[218] = (int)eventsH.dontBeModest;
		DS.TriggersEvent[301] = (int)eventsH.explainedSimulations;
		DS.TriggersEvent[302] = (int)eventsH.explainedSimulations;
		DS.TriggersEvent[72] = (int)eventsH.asked1A; DS.TriggersEvent[113] = (int)eventsH.asked1B;
		DS.TriggersEvent[135] = (int)eventsH.asked1E; DS.TriggersEvent[155] = (int)eventsH.asked2E;
		DS.TriggersEvent[213] = (int)eventsH.asked2A; DS.TriggersEvent[324] = (int)eventsH.asked2B;
		DS.TriggersEvent[360] = (int)eventsH.askedP1; DS.TriggersEvent[361] = (int)eventsH.askedP2;
		DS.TriggersEvent[362] = (int)eventsH.askedP3; DS.TriggersEvent[363] = (int)eventsH.askedP4;
		DS.TriggersEvent[364] = (int)eventsH.askedP5; DS.TriggersEvent[365] = (int)eventsH.asked3A;
		DS.TriggersEvent[525] = (int)eventsH.askedC1; DS.TriggersEvent[526] = (int)eventsH.askedC2;
		DS.TriggersEvent[527] = (int)eventsH.askedC3; DS.TriggersEvent[528] = (int)eventsH.askedC4;
		DS.TriggersEvent[529] = (int)eventsH.askedC5; DS.TriggersEvent[530] = (int)eventsH.askedC6;
		DS.TriggersEvent[531] = (int)eventsH.asked3B;

		DS.Finisher[47] = true; DS.Finisher[48] = true;
		DS.Finisher[11] = true; DS.TriggersEvent[11] = (int)eventsH.chatFinisherIntro;
		DS.Finisher[52] = true; DS.TriggersEvent[52] = (int)eventsH.chatFinisher;

		DS.Save(path + "JackieChats.bin");
	}
}