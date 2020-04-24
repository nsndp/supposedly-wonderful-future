using UnityEngine;
using System.Collections;
using System.IO;

public class BinGeneration_OfficeCEO : MonoBehaviour {
	string path;

	void Start() {
		path = COMMON.dataFolder + "OfficeCEO/";
		Narration(); OldMan();
		Debug.Log("OfficeCEO bins generated.");
		GetComponent<DataControlChapter5>().Init();
	}

	void Narration() {
		var NS = new NarrationStructure(); int size = 2;
		NS.Next = new int[size]; NS.TriggersEvent = new int?[size];
		NS.Finisher = new bool[size]; NS.Additive = new int[size];

		NS.Next[0] = 1; NS.Additive[1] = 1; NS.Finisher[1] = true;
		NS.TriggersEvent[0] = (int)events5.blackScreenOff;
		NS.TriggersEvent[1] = (int)events5.startDialogue;
		
		NS.Save(path + "Narration.bin");
	}

	void OldMan() {
		var DS = new DialogueStructure(); int size = 300;
		DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		//INTRO
		DS.R[0] = new int[] {1, 2};
		DS.R[1] = new int[] {3};
		DS.R[2] = new int[] {12};
		DS.R[3] = new int[] {4};
		DS.R[4] = new int[] {5};
		DS.R[5] = new int[] {6, 14};
		DS.R[6] = new int[] {7};
		DS.R[7] = new int[] {8, 9};
		DS.R[8] = new int[] {10};
		DS.R[9] = new int[] {11};
		DS.R[10] = new int[] {17};
		DS.R[11] = new int[] {17};
		DS.R[12] = new int[] {13, 14};
		DS.R[13] = new int[] {15};
		DS.R[14] = new int[] {16};
		DS.R[15] = new int[] {17};
		DS.R[16] = new int[] {17};
		DS.R[17] = new int[] {18};
		DS.R[18] = new int[] {19, 20};
		DS.R[19] = new int[] {21};
		DS.R[20] = new int[] {21};
		DS.R[21] = new int[] {22};
		DS.R[22] = new int[] {23};
		DS.R[23] = new int[] {24, 25};
		DS.R[24] = new int[] {26};
		DS.R[25] = new int[] {26};
		DS.R[26] = new int[] {27, 29};
		DS.R[27] = new int[] {28};
		DS.R[28] = new int[] {29};
		DS.R[29] = new int[] {30};
		DS.R[30] = new int[] {31};
		DS.R[31] = new int[] {32};
		DS.R[32] = new int[] {33};
		DS.R[33] = new int[] {34};
		DS.R[34] = new int[] {40, 41};
		//EXPLAINING
		DS.R[40] = new int[] {42};
		DS.R[41] = new int[] {43};
		DS.R[42] = new int[] {45, 46};
		DS.R[43] = new int[] {45, 46};
		DS.R[45] = new int[] {47};
		DS.R[46] = new int[] {47};
		DS.R[47] = new int[] {48, 49};
		DS.R[48] = new int[] {50};
		DS.R[49] = new int[] {50};
		DS.R[50] = new int[] {51};
		DS.R[51] = new int[] {52};
		DS.R[52] = new int[] {53};
		DS.R[53] = new int[] {54};
		DS.R[54] = new int[] {55, 56, 57};
		DS.R[55] = new int[] {58};
		DS.R[56] = new int[] {59};
		DS.R[57] = new int[] {60};
		DS.R[58] = new int[] {61, 63, 64};
		DS.R[59] = new int[] {61, 63, 64};
		DS.R[60] = new int[] {61, 63, 64};
		DS.R[61] = new int[] {65};
		DS.R[62] = new int[] {66};
		DS.R[63] = new int[] {67};
		DS.R[64] = new int[] {68};
		DS.R[65] = new int[] {62, 63, 64};
		DS.R[66] = new int[] {63, 64};
		DS.R[67] = new int[] {61, 62, 64};
		DS.R[68] = new int[] {69, 70};
		DS.R[69] = new int[] {71};
		DS.R[70] = new int[] {72};
		DS.R[71] = new int[] {73};
		DS.R[72] = new int[] {73};
		DS.R[73] = new int[] {74};
		DS.R[74] = new int[] {93, 90, 91, 92, 94, 95, 96, 97, 98, 99, 100};

		//QUESTIONS
		DS.R[90] = new int[] {110};
		DS.R[91] = new int[] {140};
		DS.R[92] = new int[] {170};
		DS.R[93] = new int[] {190};
		DS.R[94] = new int[] {200};
		DS.R[95] = new int[] {210};
		DS.R[96] = new int[] {215};
		DS.R[97] = new int[] {230};
		DS.R[98] = new int[] {235};
		DS.R[99] = new int[] {236};
		DS.R[100] = new int[] {250};
		//1
		DS.R[110] = new int[] {111};
		DS.R[111] = new int[] {112};
		DS.R[112] = new int[] {113};
		DS.R[113] = new int[] {114};
		DS.R[114] = new int[] {115, 116};
		DS.R[115] = new int[] {117};
		DS.R[116] = new int[] {118};
		DS.R[117] = new int[] {119, 120, 121};
		DS.R[118] = new int[] {119, 120, 121};
		DS.R[119] = new int[] {122};
		DS.R[120] = new int[] {122};
		DS.R[121] = new int[] {122};
		DS.R[122] = new int[] {123, 124, 126, 128};
		DS.R[123] = new int[] {129};
		DS.R[124] = new int[] {130};
		DS.R[125] = new int[] {131};
		DS.R[126] = new int[] {132};
		DS.R[127] = new int[] {133};
		DS.R[128] = new int[] {134};
		DS.R[129] = new int[] {124, 125, 126, 127, 128};
		DS.R[130] = new int[] {123, 125, 126, 127, 128};
		DS.R[131] = new int[] {123, 126, 127, 128};
		DS.R[132] = new int[] {123, 124, 125, 127, 128};
		DS.R[133] = new int[] {123, 124, 125, 128};
		DS.R[134] = new int[] {93, 91, 92, 94, 95, 96, 97, 98, 99, 100};
		//2
		DS.R[140] = new int[] {141, 142, 143};
		DS.R[141] = new int[] {144};
		DS.R[142] = new int[] {145};
		DS.R[143] = new int[] {146};
		DS.R[144] = new int[] {142, 143};
		DS.R[145] = new int[] {141, 143};
		DS.R[146] = new int[] {147, 150, 153};
		DS.R[147] = new int[] {154};
		DS.R[148] = new int[] {155};
		DS.R[149] = new int[] {156};
		DS.R[150] = new int[] {157};
		DS.R[151] = new int[] {158};
		DS.R[152] = new int[] {159};
		DS.R[153] = new int[] {160};
		DS.R[154] = new int[] {148, 149, 150, 151, 152, 153};
		DS.R[155] = new int[] {149, 150, 151, 152, 153};
		DS.R[156] = new int[] {148, 150, 151, 152, 153};
		DS.R[157] = new int[] {147, 148, 149, 151, 152, 153};
		DS.R[158] = new int[] {147, 148, 149, 152, 153};
		DS.R[159] = new int[] {147, 148, 149, 151, 153};
		DS.R[160] = new int[] {93, 90, 92, 94, 95, 96, 97, 98, 99, 100};
		//3
		DS.R[170] = new int[] {171, 172};
		DS.R[171] = new int[] {173};
		DS.R[172] = new int[] {174};
		DS.R[173] = new int[] {175, 176, 177, 178, 179, 180};
		DS.R[174] = new int[] {175, 176, 177, 178, 179, 180};
		DS.R[175] = new int[] {181};
		DS.R[176] = new int[] {182};
		DS.R[177] = new int[] {183};
		DS.R[178] = new int[] {184};
		DS.R[179] = new int[] {185};
		DS.R[180] = new int[] {186};
		DS.R[181] = new int[] {176, 177, 178, 179, 180};
		DS.R[182] = new int[] {175, 177, 178, 179, 180};
		DS.R[183] = new int[] {175, 176, 178, 179, 180};
		DS.R[184] = new int[] {175, 176, 177, 179, 180};
		DS.R[185] = new int[] {175, 176, 177, 178, 180};
		DS.R[186] = new int[] {93, 90, 91, 94, 95, 96, 97, 98, 99, 100};
		//4
		DS.R[190] = new int[] {191, 192, 193};
		DS.R[191] = new int[] {194};
		DS.R[192] = new int[] {195};
		DS.R[193] = new int[] {196};
		DS.R[194] = new int[] {192, 193};
		DS.R[195] = new int[] {191, 193};
		DS.R[196] = new int[] {90, 91, 92, 94, 95, 96, 97, 98, 99, 100};
		//5
		DS.R[200] = new int[] {201};
		DS.R[201] = new int[] {202};
		DS.R[202] = new int[] {203, 204};
		DS.R[203] = new int[] {205};
		DS.R[204] = new int[] {206};
		DS.R[205] = new int[] {93, 90, 91, 92, 95, 96, 97, 98, 99, 100};
		DS.R[206] = new int[] {93, 90, 91, 92, 95, 96, 97, 98, 99, 100};
		//6
		DS.R[210] = new int[] {211};
		DS.R[211] = new int[] {212};
		DS.R[212] = new int[] {93, 90, 91, 92, 94, 96, 97, 98, 99, 100};
		//7
		DS.R[215] = new int[] {216, 217, 218, 219};
		DS.R[216] = new int[] {220};
		DS.R[217] = new int[] {221};
		DS.R[218] = new int[] {222};
		DS.R[219] = new int[] {223};
		DS.R[220] = new int[] {217, 218, 219};
		DS.R[221] = new int[] {216, 218, 219};
		DS.R[222] = new int[] {216, 217, 219};
		DS.R[223] = new int[] {93, 90, 91, 92, 94, 95, 97, 98, 99, 100};
		//8-10
		DS.R[230] = new int[] {231, 232, 233};
		DS.R[231] = new int[] {234};
		DS.R[232] = new int[] {234};
		DS.R[233] = new int[] {234};
		DS.R[234] = new int[] {93, 90, 91, 92, 94, 95, 96, 98, 99, 100};
		DS.R[235] = new int[] {93, 90, 91, 92, 94, 95, 96, 97, 99, 100};
		DS.R[236] = new int[] {93, 90, 91, 92, 94, 95, 96, 97, 98, 100};

		//FINISHER
		DS.R[250] = new int[] {251, 252, 253};
		DS.R[251] = new int[] {254};
		DS.R[252] = new int[] {255};
		DS.R[253] = new int[] {256};
		DS.R[254] = new int[] {261, 262, 263};
		DS.R[255] = new int[] {261, 262, 263};
		DS.R[256] = new int[] {257, 258};
		DS.R[257] = new int[] {259};
		DS.R[258] = new int[] {260};
		DS.R[259] = new int[] {261, 262, 263};
		DS.R[260] = new int[] {261, 262, 263};
		DS.R[261] = new int[] {264};
		DS.R[262] = new int[] {265};
		DS.R[263] = new int[] {266};
		DS.R[264] = new int[] {267, 268};
		DS.R[265] = new int[] {267, 268};
		DS.R[266] = new int[] {267, 268};
		DS.R[267] = new int[] {269};
		DS.R[268] = new int[] {270};
		DS.R[269] = new int[] {271, 272};
		DS.R[270] = new int[] {271, 272};
		DS.R[271] = new int[] {273};
		DS.R[272] = new int[] {285};
		DS.R[273] = new int[] {274, 275, 276, 277, 278};
		DS.R[274] = new int[] {279};
		DS.R[275] = new int[] {280};
		DS.R[276] = new int[] {281};
		DS.R[277] = new int[] {282};
		DS.R[278] = new int[] {283};
		DS.R[279] = new int[] {284};
		DS.R[280] = new int[] {284};
		DS.R[281] = new int[] {284};
		DS.R[282] = new int[] {284};
		DS.R[283] = new int[] {284};
		DS.R[284] = new int[] {297};
		DS.R[285] = new int[] {286, 287, 288, 289, 290};
		DS.R[286] = new int[] {291};
		DS.R[287] = new int[] {292};
		DS.R[288] = new int[] {293};
		DS.R[289] = new int[] {294};
		DS.R[290] = new int[] {295};
		DS.R[291] = new int[] {296};
		DS.R[292] = new int[] {296};
		DS.R[293] = new int[] {296};
		DS.R[294] = new int[] {296};
		DS.R[295] = new int[] {296};
		DS.R[296] = new int[] {297};
		DS.R[297] = new int[] {298};
		DS.R[298] = null;

		DS.Locked[27] = true; //unlocked only if asked Q4 (about time traveling) during late night chats
		DS.Locked[62] = true; DS.UnlockingID[61] = new int[] {62};
		DS.Locked[125] = true; DS.UnlockingID[124] = new int[] {125};
		DS.Locked[127] = true; DS.UnlockingID[126] = new int[] {127};
		DS.Locked[148] = true; DS.Locked[149] = true; DS.UnlockingID[147] = new int[] {148, 149};
		DS.Locked[151] = true; DS.Locked[152] = true; DS.UnlockingID[150] = new int[] {151, 152};

		DS.TriggersEvent[17] = (int)events5.lookCloser;
		DS.TriggersEvent[100] = (int)events5.heardEnough;
		DS.TriggersEvent[271] = (int)events5.agreed;
		DS.TriggersEvent[272] = (int)events5.refused;
		DS.TriggersEvent[284] = (int)events5.gunShot;
		DS.TriggersEvent[296] = (int)events5.gunShot;

		DS.Save(path + "OldMan.bin");
	}
}
