using UnityEngine;
using System.Collections;
using System.IO;

public class BinGeneration_Office2016 : MonoBehaviour {
	string path;

	void Start() {
		path = COMMON.dataFolder + "Office2016/";
		Narration(); Other(); Jackie();
		Debug.Log("Office2016 bins generated.");
		GetComponent<DataControlPrologue>().Init();
	}

	void Narration() {
		var NS = new NarrationStructure(); int size = 6;
		NS.Next = new int[size]; NS.TriggersEvent = new int?[size];
		NS.Finisher = new bool[size]; NS.Additive = new int[size];

		NS.Next[0] = 1; NS.Next[1] = 2; NS.Next[2] = 3; NS.Next[3] = 4; NS.Next[4] = 5;
		NS.Additive[2] = 1; NS.Additive[4] = 1; NS.Additive[5] = 1;
		NS.Finisher[5] = true;
		NS.TriggersEvent[0] = (int)events0.blackScreenDisappear;
		NS.TriggersEvent[5] = (int)events0.introFinished;

		NS.Save(path + "Narration.bin");
	}

	void Other() {
		var DS = new DialogueStructure(); int size = 10;
		DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		DS.R[0] = new int[] {3, 4, 5, 6};
		DS.R[1] = new int[] {3, 4, 5, 6};
		DS.R[2] = new int[] {3, 4, 5, 6};
		DS.R[3] = new int[] {7};
		DS.R[4] = new int[] {8};
		DS.R[5] = new int[] {9};
		DS.R[6] = new int[] {0};
		DS.R[7] = new int[] {4, 5, 6};
		DS.R[8] = new int[] {3, 5, 6};
		DS.R[9] = new int[] {3, 4, 6};

		DS.TriggersEvent[3] = (int)events0.playMessage;
		DS.TriggersEvent[4] = (int)events0.playMessage;
		DS.TriggersEvent[5] = (int)events0.playMessage;
		DS.TriggersEvent[6] = (int)events0.phoneStepAway;
		DS.Finisher[6] = true;

		DS.Save(path + "Other.bin");
	}

	void Jackie() {
		var DS = new DialogueStructure(); int size = 300;
		DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		//DOORSTEP
		DS.R[0] = new int[] {1, 2};
		DS.R[1] = new int[] {3};
		DS.R[2] = new int[] {3};
		DS.R[3] = new int[] {4, 5, 6};
		DS.R[4] = new int[] {7};
		DS.R[5] = new int[] {8};
		DS.R[6] = new int[] {9};
		DS.R[7] = new int[] {10, 11, 12};
		DS.R[8] = new int[] {10, 11, 12};
		DS.R[9] = new int[] {10, 11, 12};
		DS.R[10] = new int[] {13};
		DS.R[11] = new int[] {13};
		DS.R[12] = new int[] {13};
		DS.R[13] = new int[] {14, 15};
		DS.R[14] = new int[] {16};
		DS.R[15] = new int[] {17};
		DS.R[16] = new int[] {18, 19, 20};
		DS.R[17] = new int[] {18, 19, 20};
		DS.R[18] = new int[] {25};
		DS.R[19] = new int[] {25};
		DS.R[20] = new int[] {25};

		//INTRO
		DS.R[25] = new int[] {26, 27, 28};
		DS.R[26] = new int[] {29};
		DS.R[27] = new int[] {30};
		DS.R[28] = new int[] {31};
		DS.R[29] = new int[] {32, 33, 34};
		DS.R[30] = new int[] {32, 33, 34};
		DS.R[31] = new int[] {32, 33, 34};
		DS.R[32] = new int[] {35};
		DS.R[33] = new int[] {36};
		DS.R[34] = new int[] {37};
		DS.R[35] = new int[] {38, 39};
		DS.R[36] = new int[] {38, 39};
		DS.R[37] = new int[] {38, 39};
		DS.R[38] = new int[] {40};
		DS.R[39] = new int[] {40};
		DS.R[40] = new int[] {41, 42, 43};
		DS.R[41] = new int[] {44};
		DS.R[42] = new int[] {45};
		DS.R[43] = new int[] {46};
		DS.R[44] = new int[] {47, 48, 49};
		DS.R[45] = new int[] {47, 48, 49};
		DS.R[46] = new int[] {47, 48, 49};
		DS.R[47] = new int[] {50};
		DS.R[48] = new int[] {50};
		DS.R[49] = new int[] {50};
		DS.R[50] = new int[] {51, 52, 53};
		DS.R[51] = new int[] {54};
		DS.R[52] = new int[] {55};
		DS.R[53] = new int[] {54};
		DS.R[54] = new int[] {56, 57, 58};
		DS.R[55] = new int[] {56, 57, 58};
		DS.R[56] = new int[] {76};
		DS.R[57] = new int[] {75};
		DS.R[58] = new int[] {59};
		DS.R[59] = new int[] {60, 61};
		DS.R[60] = new int[] {62};
		DS.R[61] = new int[] {66};
		DS.R[62] = new int[] {63, 64, 65};
		DS.R[63] = new int[] {70};
		DS.R[64] = new int[] {70};
		DS.R[65] = new int[] {70};
		DS.R[66] = new int[] {67, 68, 69};
		DS.R[67] = new int[] {70};
		DS.R[68] = new int[] {70};
		DS.R[69] = new int[] {70};
		DS.R[70] = new int[] {71, 72};
		DS.R[71] = new int[] {73};
		DS.R[72] = new int[] {74};
		DS.R[73] = new int[] {84, 86};
		DS.R[74] = new int[] {84, 86};
		DS.R[75] = new int[] {84, 86};
		DS.R[76] = new int[] {84, 86};

		//MAIN
		DS.R[84] = new int[] {92};
		DS.R[85] = new int[] {92};
		DS.R[86] = new int[] {125};
		DS.R[87] = new int[] {150};
		DS.R[88] = new int[] {190};
		DS.R[89] = new int[] {260};

		//question 1
		DS.R[92] = new int[] {93, 94, 95};
		DS.R[93] = new int[] {96};
		DS.R[94] = new int[] {97};
		DS.R[95] = new int[] {98};
		DS.R[96] = new int[] {99, 100, 101};
		DS.R[97] = new int[] {99, 100, 101};
		DS.R[98] = new int[] {99, 100, 101};
		DS.R[99] = new int[] {102};
		DS.R[100] = new int[] {103};
		DS.R[101] = new int[] {104};
		DS.R[102] = new int[] {105};
		DS.R[103] = new int[] {105};
		DS.R[104] = new int[] {105};
		DS.R[105] = new int[] {106};
		DS.R[106] = new int[] {107};
		DS.R[107] = new int[] {108};
		DS.R[108] = new int[] {109, 110, 111};
		DS.R[109] = new int[] {112};
		DS.R[110] = new int[] {113};
		DS.R[111] = new int[] {113};
		DS.R[112] = new int[] {114, 115};
		DS.R[113] = new int[] {114, 115};
		DS.R[114] = new int[] {116};
		DS.R[115] = new int[] {117};
		DS.R[116] = new int[] {115};
		DS.R[117] = new int[] {86, 87, 88, 89};
		//question 2
		DS.R[125] = new int[] {126, 127, 128, 129, 130, 131, 132, 133};
		DS.R[126] = new int[] {134};
		DS.R[127] = new int[] {135};
		DS.R[128] = new int[] {136};
		DS.R[129] = new int[] {139};
		DS.R[130] = new int[] {140};
		DS.R[131] = new int[] {141};
		DS.R[132] = new int[] {142};
		DS.R[133] = new int[] {143};
		DS.R[134] = new int[] {127, 128, 129, 130, 131, 132, 133};
		DS.R[135] = new int[] {126, 128, 129, 130, 131, 132, 133};
		DS.R[136] = new int[] {137};
		DS.R[137] = new int[] {138};
		DS.R[138] = new int[] {126, 127, 129, 130, 131, 132, 133};
		DS.R[139] = new int[] {126, 127, 128, 130, 131, 132, 133};
		DS.R[140] = new int[] {126, 127, 128, 129, 131, 132, 133};
		DS.R[141] = new int[] {126, 127, 128, 129, 130, 132, 133};
		DS.R[142] = new int[] {126, 127, 128, 129, 130, 131, 133};
		DS.R[143] = new int[] {144};
		DS.R[144] = new int[] {145};
		DS.R[145] = new int[] {85, 87, 88, 89};
		//question 3
		DS.R[150] = new int[] {151, 152, 154};
		DS.R[151] = new int[] {158};
		DS.R[152] = new int[] {173};
		DS.R[153] = new int[] {176};
		DS.R[154] = new int[] {177};
		DS.R[155] = new int[] {178};
		DS.R[156] = new int[] {179};
		DS.R[157] = new int[] {183};
		DS.R[158] = new int[] {159};
		DS.R[159] = new int[] {160};
		DS.R[160] = new int[] {161};
		DS.R[161] = new int[] {162};
		DS.R[162] = new int[] {163, 164, 165, 166};
		DS.R[163] = new int[] {167};
		DS.R[164] = new int[] {170};
		DS.R[165] = new int[] {171};
		DS.R[166] = new int[] {172};
		DS.R[167] = new int[] {168};
		DS.R[168] = new int[] {169};
		DS.R[169] = new int[] {164, 165, 166};
		DS.R[170] = new int[] {163, 165, 166};
		DS.R[171] = new int[] {163, 164, 166};
		DS.R[172] = new int[] {152, 153, 154, 155, 156, 157};
		DS.R[173] = new int[] {174};
		DS.R[174] = new int[] {175};
		DS.R[175] = new int[] {151, 153, 154, 155, 156, 157};
		DS.R[176] = new int[] {151, 154, 155, 156, 157};
		DS.R[177] = new int[] {151, 152, 153, 155, 156, 157};
		DS.R[178] = new int[] {151, 152, 153, 156, 157};
		DS.R[179] = new int[] {180, 181};
		DS.R[180] = new int[] {182};
		DS.R[181] = new int[] {182};
		DS.R[182] = new int[] {151, 152, 153, 155, 157};
		DS.R[183] = new int[] {85, 88, 89};

		//EXTRA
		DS.R[190] = new int[] {242, 191, 225, 240};
		DS.R[191] = new int[] {192};
		DS.R[192] = new int[] {193, 195};
		DS.R[193] = new int[] {197};
		DS.R[194] = new int[] {197};
		DS.R[195] = new int[] {210};
		DS.R[196] = new int[] {210};
		DS.R[197] = new int[] {198, 199, 200, 201, 202, 203};
		DS.R[198] = new int[] {204};
		DS.R[199] = new int[] {205};
		DS.R[200] = new int[] {206};
		DS.R[201] = new int[] {207};
		DS.R[202] = new int[] {208};
		DS.R[203] = new int[] {209};
		DS.R[204] = new int[] {199, 200, 201, 202, 203};
		DS.R[205] = new int[] {198, 200, 201, 202, 203};
		DS.R[206] = new int[] {198, 199, 201, 202, 203};
		DS.R[207] = new int[] {198, 199, 200, 202, 203};
		DS.R[208] = new int[] {198, 199, 200, 201, 203};
		DS.R[209] = new int[] {196};
		DS.R[210] = new int[] {211, 212};
		DS.R[211] = new int[] {213};
		DS.R[212] = new int[] {214};
		DS.R[213] = new int[] {212};
		DS.R[214] = new int[] {215, 216};
		DS.R[215] = new int[] {218};
		DS.R[216] = new int[] {217};
		DS.R[217] = new int[] {194};
		DS.R[218] = new int[] {219, 220};
		DS.R[219] = new int[] {221};
		DS.R[220] = new int[] {221};
		DS.R[221] = new int[] {194};
		DS.R[225] = new int[] {226};
		DS.R[226] = new int[] {227, 228, 229};
		DS.R[227] = new int[] {230};
		DS.R[228] = new int[] {230};
		DS.R[229] = new int[] {230};
		DS.R[230] = new int[] {231, 232, 233};
		DS.R[231] = new int[] {234};
		DS.R[232] = new int[] {237};
		DS.R[233] = new int[] {237};
		DS.R[234] = new int[] {235, 236};
		DS.R[235] = new int[] {237};
		DS.R[236] = new int[] {237};
		DS.R[237] = new int[] {242, 191, 240};
		DS.R[240] = new int[] {241};
		DS.R[241] = new int[] {89};
		DS.R[242] = new int[] {243};
		DS.R[243] = new int[] {191, 225, 240};

		//FINALE
		DS.R[260] = new int[] {261};
		DS.R[261] = new int[] {262};
		DS.R[262] = new int[] {263};
		DS.R[263] = new int[] {264};
		DS.R[264] = new int[] {265};
		DS.R[265] = new int[] {266};
		DS.R[266] = new int[] {267, 272};
		DS.R[267] = new int[] {273};
		DS.R[268] = new int[] {274};
		DS.R[269] = new int[] {277};
		DS.R[270] = new int[] {278};
		DS.R[271] = new int[] {279};
		DS.R[272] = new int[] {280};
		DS.R[273] = new int[] {268, 269, 270, 271, 272};
		DS.R[274] = new int[] {275};
		DS.R[275] = new int[] {276};
		DS.R[276] = new int[] {269, 270, 271, 272};
		DS.R[277] = new int[] {268, 270, 271, 272};
		DS.R[278] = new int[] {268, 269, 271, 272};
		DS.R[279] = new int[] {268, 269, 270, 272};
		DS.R[280] = new int[] {281};
		DS.R[281] = new int[] {283};
		DS.R[282] = new int[] {283};
		DS.R[283] = new int[] {284, 286};
		DS.R[284] = null;
		DS.R[285] = null;
		DS.R[286] = new int[] {287};
		DS.R[287] = new int[] {285, 286};

		DS.Locked[88] = true; DS.Locked[89] = true; //unlocked after asking questions 1-3
		DS.Locked[133] = true; //unlocked after asking 128 and 129
		DS.LockingID[84] = new int[] {85};
		DS.Locked[87] = true; DS.UnlockingID[86] = new int[] {87};
		DS.Locked[130] = true; DS.UnlockingID[84] = new int[] {130}; DS.UnlockingID[85] = new int[] {130};
		DS.Locked[153] = true; DS.UnlockingID[152] = new int[] {153};
		DS.Locked[155] = true; DS.Locked[156] = true; DS.UnlockingID[154] = new int[] {155, 156};
		DS.Locked[157] = true; DS.UnlockingID[151] = new int[] {157};
		DS.TriggersEvent[18] = (int)events0.invitingJackie;
		DS.TriggersEvent[19] = (int)events0.invitingJackie;
		DS.TriggersEvent[20] = (int)events0.invitingJackie;
		DS.TriggersEvent[58] = (int)events0.kindOfAnAss;
		DS.TriggersEvent[84] = (int)events0.askingQ1;
		DS.TriggersEvent[85] = (int)events0.askingQ1;
		DS.TriggersEvent[87] = (int)events0.askingQ3;
		DS.TriggersEvent[128] = (int)events0.askingMandatoryPartsInQ2;
		DS.TriggersEvent[129] = (int)events0.askingMandatoryPartsInQ2;
		DS.TriggersEvent[154] = (int)events0.longTermProject;
		DS.TriggersEvent[193] = (int)events0.timeMachineQ;
		DS.TriggersEvent[195] = (int)events0.darkAlleyQ;
		DS.TriggersEvent[215] = (int)events0.kindOfAnAss;
		DS.TriggersEvent[225] = (int)events0.kindOfAnAss;
		DS.TriggersEvent[267] = (int)events0.finalConvincing;
		DS.TriggersEvent[284] = (int)events0.finishPrologue;
		DS.TriggersEvent[285] = (int)events0.finishPrologue;
		DS.TriggersEvent[286] = (int)events0.zoomOutJackie;
		DS.Finisher[18] = true; DS.Finisher[19] = true; DS.Finisher[20] = true;
		DS.Finisher[284] = true; DS.Finisher[285] = true; DS.Finisher[286] = true;

		DS.Save(path + "Jackie.bin");
	}
}
