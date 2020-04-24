using UnityEngine;
using System.Collections;
using System.IO;

public class BinGeneration_DimlyLitHouse : MonoBehaviour {
	string path;

	void Start() {
		path = COMMON.dataFolder + "DimlyLitHouse/";
		Narration(); Mitty(); Woman(); Other();
		Debug.Log("DimlyLitHouse bins generated.");
		GetComponent<DataControlChapter1>().Init();
	}

	void Narration() {
		var NS = new NarrationStructure(); int size = 29;
		NS.Next = new int[size]; NS.TriggersEvent = new int?[size];
		NS.Finisher = new bool[size]; NS.Additive = new int[size];

		NS.Next[0] = 1; NS.Next[1] = 2; NS.Next[2] = 4; NS.Next[3] = 4; NS.Next[4] = 5;
		NS.Next[6] = 7; NS.Next[8] = 9;
		for (int i = 15; i < 28; i++) NS.Next[i] = i + 1;
		for (int i = 17; i < 26; i++) NS.Additive[i] = 2;
		NS.Additive[2] = 1; NS.Additive[3] = 1; NS.Additive[5] = 1; NS.Additive[7] = 1; NS.Additive[27] = 1;

		NS.TriggersEvent[2] = (int)events1.blackScreenDisappear;
		NS.TriggersEvent[3] = (int)events1.blackScreenDisappear;
		NS.TriggersEvent[5] = (int)events1.seenIntro;
		NS.TriggersEvent[7] = (int)events1.womanGotUp;
		NS.TriggersEvent[8] = (int)events1.didEuthanasia3;
		NS.TriggersEvent[25] = (int)events1.inPretense1;
		NS.TriggersEvent[27] = (int)events1.inPretense3;
		NS.TriggersEvent[28] = (int)events1.storyFinised;
		NS.Finisher[5] = true; NS.Finisher[7] = true; NS.Finisher[9] = true;
		NS.Finisher[10] = true; NS.Finisher[11] = true; NS.Finisher[12] = true;
		NS.Finisher[25] = true; NS.Finisher[28] = true;
		NS.Save(path + "Narration.bin");
	}

	void Mitty() {
		var DS = new DialogueStructure();
		int size = 160; DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		//introduction
		DS.R[0] = new int[] {1, 2};
		DS.R[1] = new int[] {3};
		DS.R[2] = new int[] {9};
		DS.R[3] = new int[] {4, 5};
		DS.R[4] = new int[] {8};
		DS.R[5] = new int[] {6};
		DS.R[6] = new int[] {7};
		DS.R[7] = new int[] {8};
		DS.R[8] = new int[] {10, 11, 12, 13};
		DS.R[9] = new int[] {10, 11, 12, 13};
		DS.R[10] = new int[] {14};
		DS.R[11] = new int[] {14};
		DS.R[12] = new int[] {14};
		DS.R[13] = new int[] {14};
		DS.R[14] = new int[] {15, 17, 18};
		DS.R[15] = new int[] {16};
		DS.R[16] = new int[] {20, 22};
		DS.R[17] = new int[] {19};
		DS.R[18] = new int[] {19};
		DS.R[19] = new int[] {20, 22};
		DS.R[20] = new int[] {21};
		DS.R[21] = new int[] {24, 26};
		DS.R[22] = new int[] {23};
		DS.R[23] = new int[] {24, 26};
		DS.R[24] = new int[] {25};
		DS.R[25] = new int[] {28, 29, 30, 31, 32};
		DS.R[26] = new int[] {27};
		DS.R[27] = new int[] {28, 29, 30, 31, 32};
		//first questions + getting the first gift
		DS.R[28] = new int[] {33};
		DS.R[29] = new int[] {34};
		DS.R[30] = new int[] {35};
		DS.R[31] = new int[] {36};
		DS.R[32] = new int[] {71};
		DS.R[33] = new int[] {29, 30, 31, 32, 50, 72};
		DS.R[34] = new int[] {28, 31, 32, 50, 72};
		DS.R[35] = new int[] {28, 31, 32, 50, 72};
		DS.R[36] = new int[] {37, 38, 39};
		DS.R[37] = new int[] {40};
		DS.R[38] = new int[] {40};
		DS.R[39] = new int[] {40};
		DS.R[40] = new int[] {41};
		DS.R[41] = new int[] {42};
		DS.R[42] = new int[] {43, 44};
		DS.R[43] = new int[] {73};
		DS.R[44] = new int[] {45};
		DS.R[45] = new int[] {46, 47, 48};
		DS.R[46] = new int[] {49};
		DS.R[47] = new int[] {49};
		DS.R[48] = new int[] {49};
		DS.R[49] = new int[] {28, 29, 30, 50};
		DS.R[50] = new int[] {51};
		DS.R[51] = new int[] {52, 53};
		DS.R[52] = new int[] {54};
		DS.R[53] = new int[] {54};
		DS.R[54] = new int[] {55, 57, 58};
		DS.R[55] = new int[] {56};
		DS.R[56] = new int[] {57, 58};
		DS.R[57] = new int[] {59};
		DS.R[58] = new int[] {59};
		DS.R[59] = new int[] {62};
		DS.R[62] = new int[] {63};
		DS.R[63] = new int[] {64};
		DS.R[64] = new int[] {65};
		DS.R[65] = new int[] {66, 67, 68};
		DS.R[66] = new int[] {69};
		DS.R[67] = new int[] {69};
		DS.R[68] = new int[] {69};
		DS.R[69] = new int[] {74};
		DS.R[71] = new int[] {28, 29, 30, 31, 72};
		DS.R[72] = new int[] {71};
		DS.R[73] = new int[] {28, 29, 30, 50};
		//getting the second gift
		DS.R[74] = new int[] {75, 76, 77};
		DS.R[75] = new int[] {78};
		DS.R[76] = new int[] {78};
		DS.R[77] = new int[] {74};
		DS.R[78] = new int[] {79, 80};
		DS.R[79] = new int[] {81};
		DS.R[80] = new int[] {81};
		DS.R[81] = new int[] {82, 83};
		DS.R[82] = new int[] {84};
		DS.R[83] = new int[] {84};
		DS.R[84] = new int[] {85, 86, 87};
		DS.R[85] = new int[] {88};
		DS.R[86] = new int[] {88};
		DS.R[87] = new int[] {88};
		DS.R[88] = new int[] {91, 92, 93};
		DS.R[91] = new int[] {94};
		DS.R[92] = new int[] {95};
		DS.R[93] = new int[] {96};
		DS.R[94] = new int[] {97, 99, 103, 104};
		DS.R[95] = new int[] {97, 99, 103, 104};
		DS.R[96] = new int[] {97, 99, 103, 104};
		DS.R[97] = new int[] {98};
		DS.R[98] = new int[] {101, 103, 104};
		DS.R[99] = new int[] {100};
		DS.R[100] = new int[] {102, 103, 104};
		DS.R[101] = new int[] {105};
		DS.R[102] = new int[] {105};
		DS.R[103] = new int[] {105};
		DS.R[104] = new int[] {105};
		DS.R[105] = new int[] {107};
		//examining Mitty more closely
		DS.R[107] = new int[] {108};
		DS.R[108] = new int[] {109};
		DS.R[109] = new int[] {110, 111};
		DS.R[110] = new int[] {112};
		DS.R[111] = new int[] {112};
		DS.R[112] = new int[] {113};
		DS.R[113] = new int[] {114};
		DS.R[114] = new int[] {115};
		DS.R[115] = new int[] {116};
		DS.R[116] = new int[] {117};
		DS.R[117] = new int[] {118};
		//approaching again
		DS.R[118] = new int[] {133, 134, 120, 121, 122, 123};
		DS.R[119] = new int[] {133, 134, 120, 121, 122, 123};
		DS.R[120] = new int[] {124};
		DS.R[121] = new int[] {129};
		DS.R[122] = new int[] {132};
		DS.R[123] = new int[] {119};
		DS.R[124] = new int[] {125, 127};
		DS.R[125] = new int[] {126};
		DS.R[126] = new int[] {133, 134, 121, 122, 123};
		DS.R[127] = new int[] {128};
		DS.R[128] = new int[] {133, 134, 121, 122, 123};
		DS.R[129] = new int[] {130};
		DS.R[130] = new int[] {131};
		DS.R[131] = new int[] {133, 134, 120, 122, 123};
		DS.R[132] = new int[] {133, 134, 120, 121, 123}; 
		//euthanasia
		DS.R[133] = new int[] {135};
		DS.R[134] = new int[] {135};
		DS.R[135] = new int[] {136, 137};
		DS.R[136] = new int[] {138};
		DS.R[137] = new int[] {139};
		DS.R[138] = new int[] {140};
		DS.R[139] = new int[] {140};
		DS.R[140] = new int[] {141};
		DS.R[141] = new int[] {142, 143};
		DS.R[142] = new int[] {144};
		DS.R[143] = new int[] {144};
		DS.R[144] = new int[] {145, 146, 147};
		DS.R[145] = new int[] {148};
		DS.R[146] = new int[] {148};
		DS.R[147] = new int[] {148};
		DS.R[148] = new int[] {149, 150};
		DS.R[149] = new int[] {151};
		DS.R[150] = new int[] {151};
		DS.R[151] = new int[] {152};
		DS.R[152] = new int[] {153};
		DS.R[153] = new int[] {154};
		DS.R[154] = new int[] {155};
		DS.R[155] = new int[] {156, 157};
		DS.R[156] = null;
		DS.R[157] = null;
		
		DS.Locked[30] = true; //unlocked after checking the front door
		DS.Locked[31] = true; //unlocked after meeting the woman
		DS.Locked[75] = true; DS.Locked[76] = true; //unlocked after giving the first gift
		DS.Locked[108] = true; //unlocked after giving the second gift
		DS.Locked[120] = true; DS.Locked[121] = true; DS.Locked[122] = true; //unlocked after hearing an explanation
		DS.Locked[133] = true; DS.Locked[134] = true; //unlocked only if agreed to euthanasia and picked the pill up
		DS.UnlockingID[32] = new int[] {72}; DS.LockingID[32] = new int[] {32};
		DS.UnlockingID[31] = new int[] {50}; DS.LockingID[31] = new int[] {32, 72};
		DS.Locked[50] = true; DS.Locked[72] = true; DS.UnlockingID[50] = new int[] {72};
		DS.LockingID[44] = new int[] {55};
		DS.Finisher[32] = true; DS.Finisher[69] = true; DS.Finisher[72] = true;
		DS.Finisher[77] = true; DS.Finisher[105] = true; DS.Finisher[117] = true;
		DS.Finisher[123] = true; DS.Finisher[156] = true; DS.Finisher[157] = true;
		DS.TriggersEvent[1] = (int)events1.metMitty; DS.TriggersEvent[2] = (int)events1.metMitty;
		DS.TriggersEvent[64] = (int)events1.gotFirstGift; DS.TriggersEvent[105] = (int)events1.gotSecondGift;
		DS.TriggersEvent[108] = (int)events1.sawMittyProperly;
		DS.TriggersEvent[156] = (int)events1.didEuthanasia1; DS.TriggersEvent[157] = (int)events1.didEuthanasia1;

		DS.Save(path + "Mitty.bin");
	}

	void Woman() {
		var DS = new DialogueStructure();
		int size = 200; DS.R = new int[size][];
		DS.Locked = new bool[size]; DS.Used = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];

		//introduction
		DS.R[0] = new int[] {1, 85};
		DS.R[1] = new int[] {2};
		DS.R[85] = new int[] {86};
		DS.R[2] = new int[] {3};
		DS.R[86] = new int[] {3};
		DS.R[3] = new int[] {4};
		DS.R[4] = new int[] {5, 6, 8, 9, 10, 11};
		DS.R[5] = new int[] {12};
		DS.R[6] = new int[] {13};
		DS.R[7] = new int[] {14};
		DS.R[8] = new int[] {19};
		DS.R[9] = new int[] {20};
		DS.R[10] = new int[] {21};
		DS.R[11] = new int[] {22};
		DS.R[12] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[13] = new int[] {7};
		DS.R[14] = new int[] {15, 17};
		DS.R[15] = new int[] {16};
		DS.R[16] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[17] = new int[] {18};
		DS.R[18] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[19] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[20] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[21] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[22] = new int[] {23, 24, 25};
		DS.R[23] = new int[] {26};
		DS.R[24] = new int[] {26};
		DS.R[25] = new int[] {27};
		DS.R[26] = new int[] {28, 30};
		DS.R[27] = new int[] {28, 30};
		DS.R[28] = new int[] {29};
		DS.R[29] = new int[] {33};
		DS.R[30] = new int[] {31};
		DS.R[31] = new int[] {33};
		DS.R[33] = new int[] {35, 36, 57, 5, 6, 8, 9, 10, 11, 75, 34};
		DS.R[34] = new int[] {33};
		//giving the first gift
		DS.R[35] = new int[] {37};
		DS.R[36] = new int[] {37};
		DS.R[37] = new int[] {38};
		DS.R[38] = new int[] {39};
		DS.R[39] = new int[] {40};
		DS.R[40] = new int[] {41};
		DS.R[41] = new int[] {42};
		DS.R[42] = new int[] {43};
		DS.R[43] = new int[] {44, 45, 46};
		DS.R[44] = new int[] {47};
		DS.R[45] = new int[] {48};
		DS.R[46] = new int[] {48};
		DS.R[47] = new int[] {49};
		DS.R[48] = new int[] {49};
		DS.R[49] = new int[] {50};
		DS.R[50] = new int[] {51, 52};
		DS.R[51] = new int[] {53};
		DS.R[52] = new int[] {53};
		DS.R[53] = new int[] {54};
		DS.R[54] = new int[] {55};
		DS.R[55] = new int[] {56};
		DS.R[56] = new int[] {33};
		//giving the second gift
		DS.R[57] = new int[] {58};
		DS.R[58] = new int[] {59, 60, 61};
		DS.R[59] = new int[] {62};
		DS.R[60] = new int[] {62};
		DS.R[61] = new int[] {62};
		DS.R[62] = new int[] {65, 66};
		DS.R[65] = new int[] {67};
		DS.R[66] = new int[] {67};
		DS.R[67] = new int[] {68, 69, 70};
		DS.R[68] = new int[] {71};
		DS.R[69] = new int[] {71};
		DS.R[70] = new int[] {71};
		DS.R[71] = new int[] {72, 73, 74};
		DS.R[72] = new int[] {90};
		DS.R[73] = new int[] {90};
		DS.R[74] = new int[] {90};
		//TV comment
		DS.R[75] = new int[] {76};
		DS.R[76] = new int[] {77};
		DS.R[77] = new int[] {16};
		//explanation
		DS.R[90] = new int[] {91, 92};
		DS.R[91] = new int[] {93};
		DS.R[92] = new int[] {94};
		DS.R[93] = new int[] {95, 96};
		DS.R[94] = new int[] {95, 96};
		DS.R[95] = new int[] {97};
		DS.R[96] = new int[] {97};
		DS.R[97] = new int[] {98, 99, 100, 101, 102, 103, 104, 122, 105};
		DS.R[98] = new int[] {106};
		DS.R[99] = new int[] {107};
		DS.R[100] = new int[] {108};
		DS.R[101] = new int[] {109};
		DS.R[102] = new int[] {112};
		DS.R[103] = new int[] {113};
		DS.R[104] = new int[] {120};
		DS.R[105] = new int[] {121};
		DS.R[106] = new int[] {99, 100, 101, 102, 103, 104, 122, 126, 105};
		DS.R[107] = new int[] {98, 100, 101, 102, 103, 104, 122, 126, 105};
		DS.R[108] = new int[] {98, 99, 101, 102, 103, 104, 122, 126, 105};
		DS.R[109] = new int[] {110};
		DS.R[110] = new int[] {111};
		DS.R[111] = new int[] {98, 99, 100, 102, 103, 104, 122, 126, 105};
		DS.R[112] = new int[] {98, 99, 100, 101, 103, 104, 122, 126, 105};
		DS.R[113] = new int[] {114, 115, 116};
		DS.R[114] = new int[] {117};
		DS.R[115] = new int[] {118};
		DS.R[116] = new int[] {119};
		DS.R[117] = new int[] {98, 99, 100, 101, 102, 104, 122, 126, 105};
		DS.R[118] = new int[] {98, 99, 100, 101, 102, 104, 122, 126, 105};
		DS.R[119] = new int[] {98, 99, 100, 101, 102, 104, 122, 126, 105};
		DS.R[120] = new int[] {98, 99, 100, 101, 102, 103, 122, 126, 105};
		DS.R[121] = new int[] {122, 126, 98, 99, 100, 101, 102, 103, 104, 105};
		DS.R[122] = new int[] {123};
		DS.R[123] = new int[] {124};
		DS.R[124] = new int[] {125};
		DS.R[125] = new int[] {126, 99, 100, 101, 102, 104, 105};
		DS.R[126] = new int[] {127};
		DS.R[127] = new int[] {128};
		DS.R[128] = new int[] {129};
		DS.R[129] = new int[] {130, 131};
		DS.R[130] = new int[] {132};
		DS.R[131] = new int[] {133};
		DS.R[132] = new int[] {134, 135, 137, 138};
		DS.R[133] = new int[] {134, 135, 137, 138};
		//convincing
		DS.R[134] = new int[] {156};
		DS.R[135] = new int[] {142};
		DS.R[136] = new int[] {140};
		DS.R[137] = new int[] {141};
		DS.R[138] = new int[] {140};
		DS.R[139] = new int[] {140};
		DS.R[140] = new int[] {134, 135, 136, 137, 138, 139};
		DS.R[141] = new int[] {134, 135, 136, 138, 139};
		DS.R[142] = new int[] {143, 144, 145};
		DS.R[143] = new int[] {147};
		DS.R[144] = new int[] {147};
		DS.R[145] = new int[] {146};
		DS.R[146] = new int[] {134, 135, 136, 137, 138, 139};
		DS.R[147] = new int[] {148};
		DS.R[148] = new int[] {149};
		DS.R[149] = new int[] {150};
		DS.R[150] = new int[] {152};
		DS.R[151] = new int[] {152};
		DS.R[152] = new int[] {153};
		DS.R[153] = new int[] {154};
		DS.R[154] = new int[] {140};
		DS.R[155] = new int[] {140};
		DS.R[156] = new int[] {157, 158, 159};
		DS.R[157] = new int[] {160};
		DS.R[158] = new int[] {168};
		DS.R[159] = new int[] {146};
		DS.R[160] = new int[] {161};
		DS.R[161] = new int[] {162};
		DS.R[162] = new int[] {163};
		DS.R[163] = new int[] {164};
		DS.R[164] = new int[] {165};
		DS.R[165] = new int[] {166};
		DS.R[166] = new int[] {167};
		DS.R[167] = null;
		DS.R[168] = new int[] {169};
		DS.R[169] = new int[] {170};
		DS.R[170] = new int[] {171};
		DS.R[171] = new int[] {172};
		DS.R[172] = new int[] {173};
		DS.R[173] = new int[] {174};
		DS.R[174] = new int[] {175};
		DS.R[175] = new int[] {176};
		DS.R[176] = new int[] {180};
		//euthanasia
		DS.R[180] = new int[] {181, 182};
		DS.R[181] = new int[] {183};
		DS.R[182] = new int[] {183};
		DS.R[183] = new int[] {184, 185};
		DS.R[184] = new int[] {186};
		DS.R[185] = new int[] {186};
		DS.R[186] = new int[] {187, 188, 189, 190};
		DS.R[187] = new int[] {191};
		DS.R[188] = new int[] {192};
		DS.R[189] = new int[] {193};
		DS.R[190] = new int[] {194};
		DS.R[191] = new int[] {189, 190};
		DS.R[192] = new int[] {189, 190};
		DS.R[193] = null;
		DS.R[194] = null;

		DS.Locked[6] = true; //unlocked after checking the front door
		DS.Locked[75] = true; //unlocked after interacting once with the turned-on TV
		DS.Locked[35] = true; DS.Locked[36] = true; //giving the first gift
		DS.Locked[57] = true; //giving the second gift
		DS.Locked[91] = true; DS.Locked[92] = true; //unlocked after examining Mitty
		DS.Locked[122] = true; //unlocked only after finding the pill
		DS.Locked[181] = true; DS.Locked[182] = true; //unlocked after doing euthanasia

		DS.Locked[7] = true; DS.UnlockingID[6] = new int[] {7};
		DS.Locked[9] = true; DS.UnlockingID[8] = new int[] {9};
		DS.Locked[34] = true; DS.UnlockingID[11] = new int[] {34};
		DS.LockingID[35] = new int[] {36}; DS.LockingID[36] = new int[] {35};
		DS.Locked[103] = true; DS.UnlockingID[101] = new int[] {103}; DS.UnlockingID[102] = new int[] {103};
		DS.Locked[126] = true; DS.UnlockingID[122] = new int[] {126};
		DS.Locked[136] = true; DS.UnlockingID[143] = new int[] {136}; DS.UnlockingID[144] = new int[] {136};
		DS.Locked[139] = true; DS.UnlockingID[138] = new int[] {139}; DS.LockingID[138] = new int[] {138};
		DS.LockingID[105] = new int[] {98};

		DS.Finisher[29] = true; DS.Finisher[31] = true;
		DS.Finisher[34] = true; DS.Finisher[56] = true;
		DS.Finisher[72] = true; DS.Finisher[73] = true; DS.Finisher[74] = true;
		DS.Finisher[105] = true; DS.Finisher[138] = true; DS.Finisher[139] = true;
		DS.Finisher[154] = true; DS.Finisher[155] = true;
		DS.Finisher[167] = true; DS.Finisher[176] = true;
		DS.Finisher[193] = true; DS.Finisher[194] = true;

		DS.TriggersEvent[6] = (int)events1.askedAboutDoor;
		DS.TriggersEvent[8] = (int)events1.askedTimeIsStill;
		DS.TriggersEvent[29] = (int)events1.metWoman;
		DS.TriggersEvent[31] = (int)events1.metWoman;
		DS.TriggersEvent[54] = (int)events1.gaveFirstGift;
		DS.TriggersEvent[65] = (int)events1.gaveSecondGift;
		DS.TriggersEvent[66] = (int)events1.gaveSecondGift;
		DS.TriggersEvent[91] = (int)events1.gotExplanation; DS.TriggersEvent[92] = (int)events1.gotExplanation;
		DS.TriggersEvent[135] = (int)events1.beenForceful;
		DS.TriggersEvent[144] = (int)events1.suckItUp;
		DS.TriggersEvent[145] = (int)events1.holdThatThoughtA; DS.TriggersEvent[159] = (int)events1.holdThatThoughtB;
		DS.TriggersEvent[157] = (int)events1.choicePointOfNoReturn; DS.TriggersEvent[158] = (int)events1.choicePointOfNoReturn;
		DS.TriggersEvent[167] = (int)events1.chosePretense;
		DS.TriggersEvent[175] = (int)events1.choseEuthanasia;
		DS.TriggersEvent[193] = (int)events1.storyFinised; DS.TriggersEvent[194] = (int)events1.storyFinised;

		DS.Save(path + "Woman.bin");
	}

	void Other() {
		var DS = new DialogueStructure();
		DS.R = new int[][] {
			new int[] {2, 1}, //0
			new int[] {0}, //1
			new int[] {7}, //2
			new int[] {8}, //3
			new int[] {9}, //4
			new int[] {10}, //5
			new int[] {11}, //6
			new int[] {3, 4, 5, 6}, //7
			new int[] {4, 5, 6}, //8
			new int[] {3, 5, 6}, //9
			new int[] {3, 4, 6}, //10
			new int[] {12}, //11
			new int[] {13}, //12
			new int[] {14}, //13
			new int[] {13}, //14
			new int[] {16}, //15
			new int[] {17}, //16
			new int[] {18}, //17
			new int[] {19}, //18
			new int[] {20}, //19
			new int[] {19}, //20
			new int[] {22, 23}, //21
			null, //22
			new int[] {21} //23
		};
		
		int size = 24;
		DS.Used = new bool[size]; DS.Locked = new bool[size];
		DS.UnlockingID = new int[size][]; DS.LockingID = new int[size][];
		DS.Finisher = new bool[size]; DS.TriggersEvent = new int?[size];
		
		DS.Finisher[1] = true; DS.Finisher[6] = true;
		DS.Finisher[12] = true; DS.Finisher[14] = true;
		DS.Finisher[14] = true; DS.Finisher[16] = true;
		DS.Finisher[18] = true; DS.Finisher[20] = true;
		DS.Finisher[22] = true;	DS.Finisher[23] = true;
		DS.TriggersEvent[2] = (int)events1.checkedFrontDoor;
		DS.TriggersEvent[3] = (int)events1.triedAllFrontDoor;
		DS.TriggersEvent[4] = (int)events1.triedAllFrontDoor;
		DS.TriggersEvent[5] = (int)events1.triedAllFrontDoor;
		DS.TriggersEvent[22] = (int)events1.finishLevel;
		
		DS.Save(path + "Other.bin");
	}
}