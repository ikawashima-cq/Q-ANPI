using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAnpiCtrlLib.consts
{
    public class DefaultRFParam
    {

        public const string defaltData = @"1,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_Gen1[0]
2,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x14,0x14,VND_RFIC_Gen1[1]
3,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_Gen1[2]
4,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_Gen1[3]
5,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_Gen1[4]
6,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_Gen1[5]
7,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_Gen1[6]
8,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x07,0x07,VND_RFIC_Gen1[7]
9,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0xFF,0xFF,VND_RFIC_Gen1[8]
10,General【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x17,0x17,VND_RFIC_Gen1[9]
11,General【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x4C,0x4C,VND_RFIC_Gen2[0]
12,General【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x58,0x58,VND_RFIC_Gen2[1]
13,General【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x64,0x64,VND_RFIC_Gen2[2]
14,General【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x63,0x63,VND_RFIC_Gen2[3]
15,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_BBPLL[0]
16,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_BBPLL[1]
17,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0xE8,0xE8,VND_RFIC_BBPLL[2]
18,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x5B,0x5B,VND_RFIC_BBPLL[3]
19,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x35,0x35,VND_RFIC_BBPLL[4]
20,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0xE0,0xE0,VND_RFIC_BBPLL[5]
21,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x10,0x10,VND_RFIC_BBPLL[6]
22,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_BBPLL[7]
23,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0xC0,0xC0,VND_RFIC_BBPLL[8]
24,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x1D,0x1D,VND_RFIC_BBPLL[9]
25,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x2E,0x2E,VND_RFIC_BBPLL[10]
26,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_BBPLL[11]
27,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_BBPLL[12]
28,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x86,0x86,VND_RFIC_BBPLL[13]
29,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_BBPLL[14]
30,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_BBPLL[15]
31,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x0C,0x4C,VND_RFIC_BBPLL[16]
32,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x58,0x58,VND_RFIC_BBPLL[17]
33,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_BBPLL[18]
34,BBPLL　レジスタ設定値,0x00,0xFF,－－－,0x1B,0x13,VND_RFIC_BBPLL[19]
35,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0xC8,0xC8,VND_RFIC_PPORT[0]
36,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_PPORT[1]
37,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0xA2,0x22,VND_RFIC_PPORT[2]
38,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_PPORT[3]
39,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_PPORT[4]
40,PARALLEL PORT　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_PPORT[5]
41,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[0]
42,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[1]
43,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[2]
44,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[3]
45,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0xFF,0xFF,VND_RFIC_ADAC[4]
46,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[5]
47,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[6]
48,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[7]
49,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[8]
50,AuxDAC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADAC[9]
51,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_AADC1[0]
52,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_AADC1[1]
53,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_AADC1[2]
54,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_AADC1[3]
55,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x10,0x10,VND_RFIC_AADC1[4]
56,AuxADC【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_AADC1[5]
57,AuxADC【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_AADC2[0]
58,AuxADC【設定②】　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_AADC2[1]
59,Control Output【設定①】　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_CO[0]
60,Control Output【設定①】　レジスタ設定値,0x00,0xFF,－－－,0xFF,0xFF,VND_RFIC_CO[1]
61,GPO　レジスタ設定値,0x00,0xFF,－－－,0x12,0x12,VND_RFIC_GPO[0]
62,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[1]
63,GPO　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_GPO[2]
64,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[3]
65,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[4]
66,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[5]
67,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[6]
68,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[7]
69,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[8]
70,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[9]
71,GPO　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GPO[10]
72,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[0]
73,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[1]
74,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[2]
75,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[3]
76,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_0-[4]
77,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_0-[5]
78,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_0-[6]
79,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_0-[7]
80,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_0-[8]
81,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_0-[9]
82,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_0-[10]
83,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_0-[11]
84,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[12]
85,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[13]
86,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_0-[14]
87,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_0-[15]
88,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_0-[16]
89,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_0-[17]
90,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_0-[18]
91,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[19]
92,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[20]
93,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_0-[21]
94,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_0-[22]
95,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_0-[23]
96,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_0-[24]
97,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[25]
98,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[26]
99,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_0-[27]
100,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_0-[28]
101,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_0-[29]
102,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_0-[30]
103,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[31]
104,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_0-[32]
105,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_0-[33]
106,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_0-[34]
107,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_0-[35]
108,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[36]
109,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[37]
110,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_0-[38]
111,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_0-[39]
112,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_0-[40]
113,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_0-[41]
114,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_0-[42]
115,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xFA,VND_RFIC_RFSyn_0-[43]
116,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_0-[44]
117,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x31,VND_RFIC_RFSyn_0-[45]
118,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[46]
119,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_0-[47]
120,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_0-[48]
121,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xFA,0x50,VND_RFIC_RFSyn_0-[49]
122,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_0-[50]
123,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x31,VND_RFIC_RFSyn_0-[51]
124,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_0-[52]
125,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_0-[53]
126,RF Synthesizer　0ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_0-[54]
127,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[0]
128,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[1]
129,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[2]
130,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[3]
131,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_1-[4]
132,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_1-[5]
133,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_1-[6]
134,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_1-[7]
135,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_1-[8]
136,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_1-[9]
137,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_1-[10]
138,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_1-[11]
139,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[12]
140,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[13]
141,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_1-[14]
142,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_1-[15]
143,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_1-[16]
144,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_1-[17]
145,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_1-[18]
146,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[19]
147,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[20]
148,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_1-[21]
149,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_1-[22]
150,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_1-[23]
151,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_1-[24]
152,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[25]
153,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[26]
154,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_1-[27]
155,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_1-[28]
156,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_1-[29]
157,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_1-[30]
158,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[31]
159,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_1-[32]
160,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_1-[33]
161,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_1-[34]
162,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_1-[35]
163,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[36]
164,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[37]
165,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_1-[38]
166,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_1-[39]
167,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_1-[40]
168,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_1-[41]
169,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_1-[42]
170,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xFA,VND_RFIC_RFSyn_1-[43]
171,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_1-[44]
172,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x35,VND_RFIC_RFSyn_1-[45]
173,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[46]
174,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_1-[47]
175,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_1-[48]
176,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xFA,0x50,VND_RFIC_RFSyn_1-[49]
177,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_1-[50]
178,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x35,0x31,VND_RFIC_RFSyn_1-[51]
179,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_1-[52]
180,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_1-[53]
181,RF Synthesizer　1ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_1-[54]
182,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[0]
183,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[1]
184,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[2]
185,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[3]
186,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_2-[4]
187,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_2-[5]
188,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_2-[6]
189,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_2-[7]
190,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_2-[8]
191,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_2-[9]
192,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_2-[10]
193,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_2-[11]
194,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[12]
195,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[13]
196,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_2-[14]
197,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_2-[15]
198,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_2-[16]
199,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_2-[17]
200,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_2-[18]
201,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[19]
202,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[20]
203,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_2-[21]
204,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_2-[22]
205,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_2-[23]
206,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_2-[24]
207,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[25]
208,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[26]
209,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_2-[27]
210,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_2-[28]
211,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_2-[29]
212,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_2-[30]
213,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[31]
214,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_2-[32]
215,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_2-[33]
216,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_2-[34]
217,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_2-[35]
218,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[36]
219,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[37]
220,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_2-[38]
221,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_2-[39]
222,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_2-[40]
223,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_2-[41]
224,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_2-[42]
225,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF9,VND_RFIC_RFSyn_2-[43]
226,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_2-[44]
227,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x39,VND_RFIC_RFSyn_2-[45]
228,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[46]
229,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_2-[47]
230,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_2-[48]
231,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xF9,0x50,VND_RFIC_RFSyn_2-[49]
232,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_2-[50]
233,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x39,0x31,VND_RFIC_RFSyn_2-[51]
234,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_2-[52]
235,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_2-[53]
236,RF Synthesizer　2ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_2-[54]
237,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[0]
238,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[1]
239,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[2]
240,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[3]
241,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_3-[4]
242,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_3-[5]
243,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_3-[6]
244,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_3-[7]
245,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_3-[8]
246,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_3-[9]
247,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_3-[10]
248,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_3-[11]
249,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[12]
250,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[13]
251,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_3-[14]
252,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_3-[15]
253,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_3-[16]
254,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_3-[17]
255,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_3-[18]
256,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[19]
257,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[20]
258,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_3-[21]
259,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_3-[22]
260,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_3-[23]
261,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_3-[24]
262,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[25]
263,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[26]
264,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_3-[27]
265,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_3-[28]
266,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_3-[29]
267,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_3-[30]
268,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[31]
269,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_3-[32]
270,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_3-[33]
271,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_3-[34]
272,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_3-[35]
273,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[36]
274,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[37]
275,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_3-[38]
276,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_3-[39]
277,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_3-[40]
278,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_3-[41]
279,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_3-[42]
280,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF9,VND_RFIC_RFSyn_3-[43]
281,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_3-[44]
282,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x3D,VND_RFIC_RFSyn_3-[45]
283,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[46]
284,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_3-[47]
285,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_3-[48]
286,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xF9,0x50,VND_RFIC_RFSyn_3-[49]
287,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_3-[50]
288,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x3D,0x31,VND_RFIC_RFSyn_3-[51]
289,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_3-[52]
290,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_3-[53]
291,RF Synthesizer　3ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_3-[54]
292,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[0]
293,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[1]
294,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[2]
295,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[3]
296,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_4-[4]
297,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_4-[5]
298,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_4-[6]
299,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_4-[7]
300,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_4-[8]
301,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_4-[9]
302,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_4-[10]
303,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_4-[11]
304,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[12]
305,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[13]
306,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_4-[14]
307,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_4-[15]
308,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_4-[16]
309,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_4-[17]
310,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_4-[18]
311,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[19]
312,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[20]
313,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_4-[21]
314,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_4-[22]
315,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_4-[23]
316,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_4-[24]
317,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[25]
318,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[26]
319,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_4-[27]
320,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_4-[28]
321,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_4-[29]
322,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_4-[30]
323,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[31]
324,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_4-[32]
325,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_4-[33]
326,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_4-[34]
327,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_4-[35]
328,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[36]
329,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[37]
330,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_4-[38]
331,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_4-[39]
332,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_4-[40]
333,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_4-[41]
334,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_4-[42]
335,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF8,VND_RFIC_RFSyn_4-[43]
336,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_4-[44]
337,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x41,VND_RFIC_RFSyn_4-[45]
338,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[46]
339,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_4-[47]
340,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_4-[48]
341,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xF8,0x50,VND_RFIC_RFSyn_4-[49]
342,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_4-[50]
343,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x41,0x31,VND_RFIC_RFSyn_4-[51]
344,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_4-[52]
345,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_4-[53]
346,RF Synthesizer　4ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_4-[54]
347,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[0]
348,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[1]
349,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[2]
350,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[3]
351,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_5-[4]
352,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_5-[5]
353,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_5-[6]
354,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_5-[7]
355,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_5-[8]
356,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_5-[9]
357,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_5-[10]
358,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_5-[11]
359,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[12]
360,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[13]
361,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_5-[14]
362,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_5-[15]
363,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_5-[16]
364,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_5-[17]
365,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_5-[18]
366,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[19]
367,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[20]
368,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_5-[21]
369,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_5-[22]
370,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_5-[23]
371,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_5-[24]
372,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[25]
373,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[26]
374,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_5-[27]
375,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_5-[28]
376,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_5-[29]
377,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_5-[30]
378,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[31]
379,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_5-[32]
380,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_5-[33]
381,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_5-[34]
382,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_5-[35]
383,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[36]
384,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[37]
385,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_5-[38]
386,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_5-[39]
387,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_5-[40]
388,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_5-[41]
389,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_5-[42]
390,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF8,VND_RFIC_RFSyn_5-[43]
391,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_5-[44]
392,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x45,VND_RFIC_RFSyn_5-[45]
393,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[46]
394,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_5-[47]
395,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_5-[48]
396,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xF8,0x50,VND_RFIC_RFSyn_5-[49]
397,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_5-[50]
398,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x45,0x31,VND_RFIC_RFSyn_5-[51]
399,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_5-[52]
400,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_5-[53]
401,RF Synthesizer　5ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_5-[54]
402,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[0]
403,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[1]
404,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[2]
405,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[3]
406,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_6-[4]
407,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_6-[5]
408,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_6-[6]
409,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_6-[7]
410,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_6-[8]
411,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_6-[9]
412,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_6-[10]
413,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_6-[11]
414,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[12]
415,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[13]
416,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_6-[14]
417,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_6-[15]
418,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_6-[16]
419,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_6-[17]
420,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_6-[18]
421,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[19]
422,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[20]
423,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_6-[21]
424,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_6-[22]
425,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_6-[23]
426,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_6-[24]
427,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[25]
428,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[26]
429,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_6-[27]
430,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_6-[28]
431,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_6-[29]
432,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_6-[30]
433,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[31]
434,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_6-[32]
435,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_6-[33]
436,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_6-[34]
437,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_6-[35]
438,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[36]
439,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[37]
440,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_6-[38]
441,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_6-[39]
442,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_6-[40]
443,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_6-[41]
444,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_6-[42]
445,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF7,VND_RFIC_RFSyn_6-[43]
446,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_6-[44]
447,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x49,VND_RFIC_RFSyn_6-[45]
448,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[46]
449,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_6-[47]
450,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_6-[48]
451,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xF7,0x50,VND_RFIC_RFSyn_6-[49]
452,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_6-[50]
453,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x49,0x31,VND_RFIC_RFSyn_6-[51]
454,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_6-[52]
455,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_6-[53]
456,RF Synthesizer　6ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_6-[54]
457,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[0]
458,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[1]
459,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[2]
460,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[3]
461,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_7-[4]
462,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_7-[5]
463,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_7-[6]
464,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_7-[7]
465,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_7-[8]
466,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_7-[9]
467,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_7-[10]
468,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_7-[11]
469,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[12]
470,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[13]
471,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_7-[14]
472,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_7-[15]
473,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_7-[16]
474,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_7-[17]
475,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_7-[18]
476,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[19]
477,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[20]
478,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_7-[21]
479,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_7-[22]
480,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_7-[23]
481,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_7-[24]
482,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[25]
483,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[26]
484,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_7-[27]
485,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_7-[28]
486,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_7-[29]
487,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_7-[30]
488,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[31]
489,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_7-[32]
490,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_7-[33]
491,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_7-[34]
492,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_7-[35]
493,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[36]
494,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[37]
495,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_7-[38]
496,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_7-[39]
497,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_7-[40]
498,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_7-[41]
499,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_7-[42]
500,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF7,VND_RFIC_RFSyn_7-[43]
501,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_7-[44]
502,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x4D,VND_RFIC_RFSyn_7-[45]
503,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[46]
504,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_7-[47]
505,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_7-[48]
506,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xF7,0x50,VND_RFIC_RFSyn_7-[49]
507,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_7-[50]
508,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x4D,0x31,VND_RFIC_RFSyn_7-[51]
509,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_7-[52]
510,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_7-[53]
511,RF Synthesizer　7ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_7-[54]
512,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[0]
513,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[1]
514,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[2]
515,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[3]
516,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_8-[4]
517,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_8-[5]
518,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_8-[6]
519,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_8-[7]
520,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_8-[8]
521,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_8-[9]
522,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_8-[10]
523,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_8-[11]
524,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[12]
525,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[13]
526,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_8-[14]
527,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_8-[15]
528,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_8-[16]
529,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_8-[17]
530,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_8-[18]
531,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[19]
532,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[20]
533,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_8-[21]
534,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_8-[22]
535,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_8-[23]
536,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_8-[24]
537,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[25]
538,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[26]
539,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_8-[27]
540,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_8-[28]
541,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_8-[29]
542,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_8-[30]
543,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[31]
544,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_8-[32]
545,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_8-[33]
546,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_8-[34]
547,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_8-[35]
548,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[36]
549,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[37]
550,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_8-[38]
551,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_8-[39]
552,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_8-[40]
553,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_8-[41]
554,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_8-[42]
555,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF6,VND_RFIC_RFSyn_8-[43]
556,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_8-[44]
557,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x51,VND_RFIC_RFSyn_8-[45]
558,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[46]
559,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_8-[47]
560,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_8-[48]
561,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xF6,0x50,VND_RFIC_RFSyn_8-[49]
562,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_8-[50]
563,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x51,0x31,VND_RFIC_RFSyn_8-[51]
564,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_8-[52]
565,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_8-[53]
566,RF Synthesizer　8ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_8-[54]
567,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[0]
568,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[1]
569,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[2]
570,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[3]
571,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_9-[4]
572,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_9-[5]
573,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_9-[6]
574,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_9-[7]
575,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_9-[8]
576,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_9-[9]
577,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_9-[10]
578,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_9-[11]
579,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[12]
580,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[13]
581,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_9-[14]
582,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_9-[15]
583,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_9-[16]
584,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_9-[17]
585,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_9-[18]
586,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[19]
587,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[20]
588,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_9-[21]
589,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_9-[22]
590,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_9-[23]
591,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_9-[24]
592,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[25]
593,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[26]
594,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_9-[27]
595,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_9-[28]
596,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_9-[29]
597,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_9-[30]
598,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[31]
599,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_9-[32]
600,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_9-[33]
601,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_9-[34]
602,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_9-[35]
603,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[36]
604,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[37]
605,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_9-[38]
606,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_9-[39]
607,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_9-[40]
608,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_9-[41]
609,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_9-[42]
610,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF6,VND_RFIC_RFSyn_9-[43]
611,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_9-[44]
612,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x55,VND_RFIC_RFSyn_9-[45]
613,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[46]
614,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_9-[47]
615,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_9-[48]
616,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xF6,0x50,VND_RFIC_RFSyn_9-[49]
617,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_9-[50]
618,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0x31,VND_RFIC_RFSyn_9-[51]
619,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_9-[52]
620,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_9-[53]
621,RF Synthesizer　9ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_9-[54]
622,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[0]
623,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[1]
624,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[2]
625,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[3]
626,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_10-[4]
627,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_10-[5]
628,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_10-[6]
629,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_10-[7]
630,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_10-[8]
631,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_10-[9]
632,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_10-[10]
633,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_10-[11]
634,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[12]
635,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[13]
636,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_10-[14]
637,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_10-[15]
638,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_10-[16]
639,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_10-[17]
640,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_10-[18]
641,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[19]
642,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[20]
643,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_10-[21]
644,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_10-[22]
645,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_10-[23]
646,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_10-[24]
647,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[25]
648,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[26]
649,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_10-[27]
650,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_10-[28]
651,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_10-[29]
652,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_10-[30]
653,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[31]
654,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_10-[32]
655,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_10-[33]
656,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_10-[34]
657,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_10-[35]
658,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[36]
659,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[37]
660,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_10-[38]
661,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_10-[39]
662,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_10-[40]
663,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_10-[41]
664,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_10-[42]
665,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF5,VND_RFIC_RFSyn_10-[43]
666,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_10-[44]
667,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x59,VND_RFIC_RFSyn_10-[45]
668,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[46]
669,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_10-[47]
670,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_10-[48]
671,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xF5,0x50,VND_RFIC_RFSyn_10-[49]
672,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_10-[50]
673,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x59,0x31,VND_RFIC_RFSyn_10-[51]
674,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_10-[52]
675,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_10-[53]
676,RF Synthesizer　10ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_10-[54]
677,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[0]
678,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[1]
679,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[2]
680,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[3]
681,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_11-[4]
682,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_11-[5]
683,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_11-[6]
684,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_11-[7]
685,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_11-[8]
686,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_11-[9]
687,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_11-[10]
688,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_11-[11]
689,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[12]
690,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[13]
691,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_11-[14]
692,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_11-[15]
693,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_11-[16]
694,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_11-[17]
695,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_11-[18]
696,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[19]
697,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[20]
698,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_11-[21]
699,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_11-[22]
700,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_11-[23]
701,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_11-[24]
702,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[25]
703,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[26]
704,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_11-[27]
705,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_11-[28]
706,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_11-[29]
707,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_11-[30]
708,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[31]
709,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_11-[32]
710,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_11-[33]
711,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_11-[34]
712,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_11-[35]
713,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[36]
714,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[37]
715,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_11-[38]
716,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_11-[39]
717,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_11-[40]
718,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_11-[41]
719,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_11-[42]
720,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF5,VND_RFIC_RFSyn_11-[43]
721,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_11-[44]
722,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x5D,VND_RFIC_RFSyn_11-[45]
723,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[46]
724,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_11-[47]
725,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_11-[48]
726,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xF5,0x50,VND_RFIC_RFSyn_11-[49]
727,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_11-[50]
728,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x5D,0x31,VND_RFIC_RFSyn_11-[51]
729,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_11-[52]
730,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_11-[53]
731,RF Synthesizer　11ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_11-[54]
732,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[0]
733,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[1]
734,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[2]
735,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[3]
736,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_12-[4]
737,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_12-[5]
738,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_12-[6]
739,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_12-[7]
740,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_12-[8]
741,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_12-[9]
742,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_12-[10]
743,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_12-[11]
744,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[12]
745,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[13]
746,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_12-[14]
747,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_12-[15]
748,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_12-[16]
749,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_12-[17]
750,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_12-[18]
751,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[19]
752,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[20]
753,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_12-[21]
754,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_12-[22]
755,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_12-[23]
756,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_12-[24]
757,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[25]
758,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[26]
759,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_12-[27]
760,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_12-[28]
761,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_12-[29]
762,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_12-[30]
763,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[31]
764,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_12-[32]
765,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_12-[33]
766,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_12-[34]
767,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_12-[35]
768,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[36]
769,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[37]
770,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_12-[38]
771,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_12-[39]
772,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_12-[40]
773,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_12-[41]
774,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_12-[42]
775,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF5,VND_RFIC_RFSyn_12-[43]
776,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_12-[44]
777,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x61,VND_RFIC_RFSyn_12-[45]
778,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[46]
779,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_12-[47]
780,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_12-[48]
781,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xF5,0x50,VND_RFIC_RFSyn_12-[49]
782,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_12-[50]
783,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x61,0x31,VND_RFIC_RFSyn_12-[51]
784,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_12-[52]
785,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_12-[53]
786,RF Synthesizer　12ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_12-[54]
787,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[0]
788,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[1]
789,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[2]
790,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[3]
791,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_13-[4]
792,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RFSyn_13-[5]
793,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_13-[6]
794,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x8E,VND_RFIC_RFSyn_13-[7]
795,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_13-[8]
796,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RFSyn_13-[9]
797,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_13-[10]
798,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RFSyn_13-[11]
799,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[12]
800,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[13]
801,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_13-[14]
802,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_RFSyn_13-[15]
803,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_RFSyn_13-[16]
804,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_13-[17]
805,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_RFSyn_13-[18]
806,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[19]
807,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[20]
808,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_13-[21]
809,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_13-[22]
810,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_13-[23]
811,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_13-[24]
812,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[25]
813,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[26]
814,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_13-[27]
815,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x8E,0x91,VND_RFIC_RFSyn_13-[28]
816,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xC3,0xC3,VND_RFIC_RFSyn_13-[29]
817,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_13-[30]
818,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[31]
819,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x4A,VND_RFIC_RFSyn_13-[32]
820,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xC1,0xC1,VND_RFIC_RFSyn_13-[33]
821,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RFSyn_13-[34]
822,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_RFSyn_13-[35]
823,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[36]
824,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[37]
825,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_RFSyn_13-[38]
826,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x84,0x8E,VND_RFIC_RFSyn_13-[39]
827,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xC7,0xC3,VND_RFIC_RFSyn_13-[40]
828,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xEF,0xEF,VND_RFIC_RFSyn_13-[41]
829,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_RFSyn_13-[42]
830,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x50,0xF4,VND_RFIC_RFSyn_13-[43]
831,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x55,0xFF,VND_RFIC_RFSyn_13-[44]
832,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x31,0x65,VND_RFIC_RFSyn_13-[45]
833,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[46]
834,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xE4,0xD0,VND_RFIC_RFSyn_13-[47]
835,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_13-[48]
836,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xF4,0x50,VND_RFIC_RFSyn_13-[49]
837,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xFF,0x55,VND_RFIC_RFSyn_13-[50]
838,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x65,0x31,VND_RFIC_RFSyn_13-[51]
839,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RFSyn_13-[52]
840,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0xD0,0xE4,VND_RFIC_RFSyn_13-[53]
841,RF Synthesizer　13ch　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_RFSyn_13-[54]
842,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_MST[0]
843,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0F,0x0F,VND_RFIC_MST[1]
844,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x78,0x78,VND_RFIC_MST[2]
845,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[3]
846,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[4]
847,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[5]
848,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[6]
849,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[7]
850,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_MST[8]
851,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x74,0x74,VND_RFIC_MST[9]
852,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[10]
853,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_MST[11]
854,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[12]
855,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[13]
856,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[14]
857,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_MST[15]
858,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x70,0x70,VND_RFIC_MST[16]
859,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[17]
860,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x15,0x15,VND_RFIC_MST[18]
861,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[19]
862,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[20]
863,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[21]
864,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0C,0x0C,VND_RFIC_MST[22]
865,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x6C,0x6C,VND_RFIC_MST[23]
866,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[24]
867,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x1B,0x1B,VND_RFIC_MST[25]
868,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[26]
869,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[27]
870,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[28]
871,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_MST[29]
872,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x68,0x68,VND_RFIC_MST[30]
873,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[31]
874,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x21,0x21,VND_RFIC_MST[32]
875,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[33]
876,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[34]
877,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[35]
878,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x0A,0x0A,VND_RFIC_MST[36]
879,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x64,0x64,VND_RFIC_MST[37]
880,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[38]
881,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x25,0x25,VND_RFIC_MST[39]
882,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[40]
883,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[41]
884,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[42]
885,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x09,0x09,VND_RFIC_MST[43]
886,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x60,0x60,VND_RFIC_MST[44]
887,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[45]
888,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x29,0x29,VND_RFIC_MST[46]
889,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[47]
890,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[48]
891,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[49]
892,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_MST[50]
893,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x5C,0x5C,VND_RFIC_MST[51]
894,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[52]
895,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x2C,0x2C,VND_RFIC_MST[53]
896,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[54]
897,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[55]
898,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[56]
899,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x07,0x07,VND_RFIC_MST[57]
900,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x58,0x58,VND_RFIC_MST[58]
901,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[59]
902,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x2F,0x2F,VND_RFIC_MST[60]
903,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[61]
904,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[62]
905,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[63]
906,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[64]
907,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x54,0x54,VND_RFIC_MST[65]
908,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[66]
909,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x31,0x31,VND_RFIC_MST[67]
910,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[68]
911,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[69]
912,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[70]
913,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_MST[71]
914,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x50,0x50,VND_RFIC_MST[72]
915,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[73]
916,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x33,0x33,VND_RFIC_MST[74]
917,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[75]
918,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[76]
919,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[77]
920,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_MST[78]
921,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x4C,0x4C,VND_RFIC_MST[79]
922,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[80]
923,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x34,0x34,VND_RFIC_MST[81]
924,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[82]
925,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[83]
926,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[84]
927,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_MST[85]
928,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x48,0x48,VND_RFIC_MST[86]
929,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[87]
930,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x35,0x35,VND_RFIC_MST[88]
931,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[89]
932,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[90]
933,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[91]
934,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_MST[92]
935,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x30,0x30,VND_RFIC_MST[93]
936,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[94]
937,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x3A,0x3A,VND_RFIC_MST[95]
938,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[96]
939,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[97]
940,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[98]
941,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_MST[99]
942,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_MST[100]
943,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[101]
944,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x3D,0x3D,VND_RFIC_MST[102]
945,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[103]
946,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[104]
947,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[105]
948,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[106]
949,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[107]
950,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[108]
951,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x3E,0x3E,VND_RFIC_MST[109]
952,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_MST[110]
953,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[111]
954,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[112]
955,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_MST[113]
956,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[114]
957,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[115]
958,MIx Sub-Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MST[116]
959,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1A,0x1A,VND_RFIC_GT[0]
960,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[1]
961,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[2]
962,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[3]
963,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[4]
964,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[5]
965,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[6]
966,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[7]
967,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_GT[8]
968,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_GT[9]
969,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[10]
970,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[11]
971,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[12]
972,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[13]
973,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[14]
974,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_GT[15]
975,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_GT[16]
976,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[17]
977,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[18]
978,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[19]
979,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[20]
980,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[21]
981,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_GT[22]
982,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_GT[23]
983,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[24]
984,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[25]
985,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[26]
986,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[27]
987,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[28]
988,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_GT[29]
989,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_GT[30]
990,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[31]
991,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[32]
992,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[33]
993,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[34]
994,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[35]
995,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_GT[36]
996,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x05,0x05,VND_RFIC_GT[37]
997,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[38]
998,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[39]
999,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[40]
1000,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[41]
1001,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[42]
1002,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_GT[43]
1003,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x06,0x06,VND_RFIC_GT[44]
1004,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[45]
1005,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[46]
1006,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[47]
1007,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[48]
1008,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[49]
1009,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x07,0x07,VND_RFIC_GT[50]
1010,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x07,0x07,VND_RFIC_GT[51]
1011,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[52]
1012,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[53]
1013,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[54]
1014,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[55]
1015,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[56]
1016,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_GT[57]
1017,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_GT[58]
1018,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[59]
1019,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[60]
1020,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[61]
1021,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[62]
1022,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[63]
1023,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x09,0x09,VND_RFIC_GT[64]
1024,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x09,0x09,VND_RFIC_GT[65]
1025,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[66]
1026,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[67]
1027,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[68]
1028,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[69]
1029,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[70]
1030,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0A,0x0A,VND_RFIC_GT[71]
1031,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0A,0x0A,VND_RFIC_GT[72]
1032,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[73]
1033,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[74]
1034,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[75]
1035,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[76]
1036,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[77]
1037,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_GT[78]
1038,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0B,0x0B,VND_RFIC_GT[79]
1039,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[80]
1040,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[81]
1041,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[82]
1042,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[83]
1043,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[84]
1044,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0C,0x0C,VND_RFIC_GT[85]
1045,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0C,0x0C,VND_RFIC_GT[86]
1046,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[87]
1047,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[88]
1048,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[89]
1049,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[90]
1050,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[91]
1051,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_GT[92]
1052,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_GT[93]
1053,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[94]
1054,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[95]
1055,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[96]
1056,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[97]
1057,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[98]
1058,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_GT[99]
1059,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x23,0x23,VND_RFIC_GT[100]
1060,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[101]
1061,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[102]
1062,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[103]
1063,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[104]
1064,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[105]
1065,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x0F,0x0F,VND_RFIC_GT[106]
1066,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x24,0x24,VND_RFIC_GT[107]
1067,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[108]
1068,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[109]
1069,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[110]
1070,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[111]
1071,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[112]
1072,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x10,0x10,VND_RFIC_GT[113]
1073,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x43,0x43,VND_RFIC_GT[114]
1074,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[115]
1075,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[116]
1076,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[117]
1077,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[118]
1078,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[119]
1079,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x11,0x11,VND_RFIC_GT[120]
1080,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x44,0x44,VND_RFIC_GT[121]
1081,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[122]
1082,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[123]
1083,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[124]
1084,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[125]
1085,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[126]
1086,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x12,0x12,VND_RFIC_GT[127]
1087,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x45,0x45,VND_RFIC_GT[128]
1088,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[129]
1089,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[130]
1090,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[131]
1091,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[132]
1092,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[133]
1093,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x13,0x13,VND_RFIC_GT[134]
1094,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x46,0x46,VND_RFIC_GT[135]
1095,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[136]
1096,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[137]
1097,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[138]
1098,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[139]
1099,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[140]
1100,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x14,0x14,VND_RFIC_GT[141]
1101,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x47,0x47,VND_RFIC_GT[142]
1102,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[143]
1103,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[144]
1104,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[145]
1105,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[146]
1106,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[147]
1107,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x15,0x15,VND_RFIC_GT[148]
1108,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x48,0x48,VND_RFIC_GT[149]
1109,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[150]
1110,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[151]
1111,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[152]
1112,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[153]
1113,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[154]
1114,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x16,0x16,VND_RFIC_GT[155]
1115,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x43,0x43,VND_RFIC_GT[156]
1116,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[157]
1117,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[158]
1118,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[159]
1119,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[160]
1120,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[161]
1121,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x17,0x17,VND_RFIC_GT[162]
1122,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x44,0x44,VND_RFIC_GT[163]
1123,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[164]
1124,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[165]
1125,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[166]
1126,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[167]
1127,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[168]
1128,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x18,0x18,VND_RFIC_GT[169]
1129,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x45,0x45,VND_RFIC_GT[170]
1130,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[171]
1131,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[172]
1132,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[173]
1133,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[174]
1134,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[175]
1135,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x19,0x19,VND_RFIC_GT[176]
1136,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x46,0x46,VND_RFIC_GT[177]
1137,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[178]
1138,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[179]
1139,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[180]
1140,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[181]
1141,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[182]
1142,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1A,0x1A,VND_RFIC_GT[183]
1143,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x47,0x47,VND_RFIC_GT[184]
1144,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[185]
1145,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[186]
1146,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[187]
1147,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[188]
1148,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[189]
1149,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1B,0x1B,VND_RFIC_GT[190]
1150,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x48,0x48,VND_RFIC_GT[191]
1151,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[192]
1152,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[193]
1153,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[194]
1154,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[195]
1155,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[196]
1156,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1C,0x1C,VND_RFIC_GT[197]
1157,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x63,0x63,VND_RFIC_GT[198]
1158,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[199]
1159,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[200]
1160,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[201]
1161,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[202]
1162,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[203]
1163,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1D,0x1D,VND_RFIC_GT[204]
1164,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x64,0x64,VND_RFIC_GT[205]
1165,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[206]
1166,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[207]
1167,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[208]
1168,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[209]
1169,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[210]
1170,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[211]
1171,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x65,0x65,VND_RFIC_GT[212]
1172,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[213]
1173,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[214]
1174,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[215]
1175,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[216]
1176,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[217]
1177,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1F,0x1F,VND_RFIC_GT[218]
1178,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x66,0x66,VND_RFIC_GT[219]
1179,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[220]
1180,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[221]
1181,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[222]
1182,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[223]
1183,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[224]
1184,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[225]
1185,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x67,0x67,VND_RFIC_GT[226]
1186,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[227]
1187,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[228]
1188,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[229]
1189,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[230]
1190,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[231]
1191,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x21,0x21,VND_RFIC_GT[232]
1192,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x68,0x68,VND_RFIC_GT[233]
1193,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[234]
1194,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[235]
1195,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[236]
1196,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[237]
1197,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[238]
1198,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x22,0x22,VND_RFIC_GT[239]
1199,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x69,0x69,VND_RFIC_GT[240]
1200,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[241]
1201,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[242]
1202,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[243]
1203,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[244]
1204,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[245]
1205,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x23,0x23,VND_RFIC_GT[246]
1206,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6A,0x6A,VND_RFIC_GT[247]
1207,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[248]
1208,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[249]
1209,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[250]
1210,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[251]
1211,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[252]
1212,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x24,0x24,VND_RFIC_GT[253]
1213,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6B,0x6B,VND_RFIC_GT[254]
1214,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[255]
1215,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[256]
1216,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[257]
1217,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[258]
1218,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[259]
1219,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x25,0x25,VND_RFIC_GT[260]
1220,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6C,0x6C,VND_RFIC_GT[261]
1221,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[262]
1222,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[263]
1223,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[264]
1224,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[265]
1225,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[266]
1226,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x26,0x26,VND_RFIC_GT[267]
1227,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6D,0x6D,VND_RFIC_GT[268]
1228,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[269]
1229,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[270]
1230,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[271]
1231,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[272]
1232,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[273]
1233,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x27,0x27,VND_RFIC_GT[274]
1234,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6E,0x6E,VND_RFIC_GT[275]
1235,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[276]
1236,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[277]
1237,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[278]
1238,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[279]
1239,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[280]
1240,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x28,0x28,VND_RFIC_GT[281]
1241,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x6F,0x6F,VND_RFIC_GT[282]
1242,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x38,0x38,VND_RFIC_GT[283]
1243,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_GT[284]
1244,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1E,VND_RFIC_GT[285]
1245,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[286]
1246,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[287]
1247,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x1A,0x1A,VND_RFIC_GT[288]
1248,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[289]
1249,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[290]
1250,Rx Gain Table　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_GT[291]
1251,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0xE0,0xE0,VND_RFIC_MGC[0]
1252,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_MGC[1]
1253,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x23,0x23,VND_RFIC_MGC[2]
1254,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x4C,0x4C,VND_RFIC_MGC[3]
1255,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x44,0x44,VND_RFIC_MGC[4]
1256,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x7F,0x7F,VND_RFIC_MGC[5]
1257,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x2F,0x2F,VND_RFIC_MGC[6]
1258,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x3A,0x3A,VND_RFIC_MGC[7]
1259,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x08,0x08,VND_RFIC_MGC[8]
1260,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x1F,0x1F,VND_RFIC_MGC[9]
1261,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x1C,0x1C,VND_RFIC_MGC[10]
1262,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x14,0x00,VND_RFIC_MGC[11]
1263,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x1B,0x03,VND_RFIC_MGC[12]
1264,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x30,0x30,VND_RFIC_MGC[13]
1265,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x0C,0x0C,VND_RFIC_MGC[14]
1266,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_MGC[15]
1267,Rx Manual Gain Control　レジスタ設定値,0x00,0xFF,－－－,0x13,0x13,VND_RFIC_MGC[16]
1268,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x01,0x00,VND_RFIC_RxBBF[0]
1269,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x66,0x1A,VND_RFIC_RxBBF[1]
1270,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x28,0x64,VND_RFIC_RxBBF[2]
1271,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x1F,VND_RFIC_RxBBF[3]
1272,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x3F,0x3F,VND_RFIC_RxBBF[4]
1273,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_RxBBF[5]
1274,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RxBBF[6]
1275,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_RxBBF[7]
1276,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x80,0x80,VND_RFIC_RxBBF[8]
1277,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_RxBBF[9]
1278,Rx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_RxBBF[10]
1279,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x37,0x23,VND_RFIC_TxBBF[0]
1280,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x1F,0x1E,VND_RFIC_TxBBF[1]
1281,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x00,0x01,VND_RFIC_TxBBF[2]
1282,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x1A,0x66,VND_RFIC_TxBBF[3]
1283,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x22,0x22,VND_RFIC_TxBBF[4]
1284,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_TxBBF[5]
1285,Tx BB Filter　レジスタ設定値,0x00,0xFF,－－－,0x26,0x26,VND_RFIC_TxBBF[6]
1286,Rx TIA　レジスタ設定値,0x00,0xFF,－－－,0xE0,0xE0,VND_RFIC_RxTIA[0]
1287,Rx TIA　レジスタ設定値,0x00,0xFF,－－－,0x1E,0x7F,VND_RFIC_RxTIA[1]
1288,Rx TIA　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_RxTIA[2]
1289,Tx2nd Filter　レジスタ設定値,0x00,0xFF,－－－,0x3F,0x20,VND_RFIC_Tx2nd[0]
1290,Tx2nd Filter　レジスタ設定値,0x00,0xFF,－－－,0x01,0x03,VND_RFIC_Tx2nd[1]
1291,Tx2nd Filter　レジスタ設定値,0x00,0xFF,－－－,0x59,0x59,VND_RFIC_Tx2nd[2]
1292,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[0]
1293,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[1]
1294,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[2]
1295,ADC　レジスタ設定値,0x00,0xFF,－－－,0x24,0x24,VND_RFIC_ADC[3]
1296,ADC　レジスタ設定値,0x00,0xFF,－－－,0x24,0x24,VND_RFIC_ADC[4]
1297,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[5]
1298,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[6]
1299,ADC　レジスタ設定値,0x00,0xFF,－－－,0x7A,0x69,VND_RFIC_ADC[7]
1300,ADC　レジスタ設定値,0x00,0xFF,－－－,0x81,0x96,VND_RFIC_ADC[8]
1301,ADC　レジスタ設定値,0x00,0xFF,－－－,0x3B,0x32,VND_RFIC_ADC[9]
1302,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4A,0x3F,VND_RFIC_ADC[10]
1303,ADC　レジスタ設定値,0x00,0xFF,－－－,0x51,0x5E,VND_RFIC_ADC[11]
1304,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4D,0x42,VND_RFIC_ADC[12]
1305,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4F,0x5C,VND_RFIC_ADC[13]
1306,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[14]
1307,ADC　レジスタ設定値,0x00,0xFF,－－－,0x7D,0x6C,VND_RFIC_ADC[15]
1308,ADC　レジスタ設定値,0x00,0xFF,－－－,0x7D,0x6C,VND_RFIC_ADC[16]
1309,ADC　レジスタ設定値,0x00,0xFF,－－－,0x7D,0x6C,VND_RFIC_ADC[17]
1310,ADC　レジスタ設定値,0x00,0xFF,－－－,0x48,0x3D,VND_RFIC_ADC[18]
1311,ADC　レジスタ設定値,0x00,0xFF,－－－,0x48,0x3D,VND_RFIC_ADC[19]
1312,ADC　レジスタ設定値,0x00,0xFF,－－－,0x48,0x3D,VND_RFIC_ADC[20]
1313,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4B,0x40,VND_RFIC_ADC[21]
1314,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4B,0x40,VND_RFIC_ADC[22]
1315,ADC　レジスタ設定値,0x00,0xFF,－－－,0x4B,0x40,VND_RFIC_ADC[23]
1316,ADC　レジスタ設定値,0x00,0xFF,－－－,0x2E,0x2E,VND_RFIC_ADC[24]
1317,ADC　レジスタ設定値,0x00,0xFF,－－－,0x8C,0x8B,VND_RFIC_ADC[25]
1318,ADC　レジスタ設定値,0x00,0xFF,－－－,0x10,0x0F,VND_RFIC_ADC[26]
1319,ADC　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0D,VND_RFIC_ADC[27]
1320,ADC　レジスタ設定値,0x00,0xFF,－－－,0x8C,0x8B,VND_RFIC_ADC[28]
1321,ADC　レジスタ設定値,0x00,0xFF,－－－,0x10,0x0F,VND_RFIC_ADC[29]
1322,ADC　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0D,VND_RFIC_ADC[30]
1323,ADC　レジスタ設定値,0x00,0xFF,－－－,0x8C,0x8B,VND_RFIC_ADC[31]
1324,ADC　レジスタ設定値,0x00,0xFF,－－－,0x10,0x0F,VND_RFIC_ADC[32]
1325,ADC　レジスタ設定値,0x00,0xFF,－－－,0x1B,0x1A,VND_RFIC_ADC[33]
1326,ADC　レジスタ設定値,0x00,0xFF,－－－,0x1C,0x1A,VND_RFIC_ADC[34]
1327,ADC　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_ADC[35]
1328,ADC　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_ADC[36]
1329,ADC　レジスタ設定値,0x00,0xFF,－－－,0x2C,0x2C,VND_RFIC_ADC[37]
1330,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[38]
1331,ADC　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_ADC[39]
1332,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x15,VND_RFIC_TxQC[0]
1333,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x40,0x00,VND_RFIC_TxQC[1]
1334,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x79,0x79,VND_RFIC_TxQC[2]
1335,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0xFF,0xFF,VND_RFIC_TxQC[3]
1336,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x4F,0x4F,VND_RFIC_TxQC[4]
1337,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_TxQC[5]
1338,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_TxQC[6]
1339,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x33,0x33,VND_RFIC_TxQC[7]
1340,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_TxQC[8]
1341,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x3F,0x3F,VND_RFIC_TxQC[9]
1342,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x0F,0x0F,VND_RFIC_TxQC[10]
1343,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_TxQC[11]
1344,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_TxQC[12]
1345,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x20,0x20,VND_RFIC_TxQC[13]
1346,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x10,0x10,VND_RFIC_TxQC[14]
1347,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x14,0x14,VND_RFIC_TxQC[15]
1348,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x83,0x83,VND_RFIC_TxQC[16]
1349,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x64,0x64,VND_RFIC_TxQC[17]
1350,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x30,0x30,VND_RFIC_TxQC[18]
1351,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x02,0x02,VND_RFIC_TxQC[19]
1352,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x10,0x10,VND_RFIC_TxQC[20]
1353,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x03,0x03,VND_RFIC_TxQC[21]
1354,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x25,0x25,VND_RFIC_TxQC[22]
1355,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x75,0x75,VND_RFIC_TxQC[23]
1356,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x95,0x95,VND_RFIC_TxQC[24]
1357,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0xCF,0xCF,VND_RFIC_TxQC[25]
1358,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0xAD,0xAD,VND_RFIC_TxQC[26]
1359,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0xA2,0x22,VND_RFIC_TxQC[27]
1360,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x01,0x01,VND_RFIC_TxQC[28]
1361,Tx Quadrature Calibration　レジスタ設定値,0x00,0xFF,－－－,0x04,0x04,VND_RFIC_TxQC[29]
1362,Tx ATT　レジスタ設定値,0x00,0xFF,－－－,0x24,0x9C,VND_RFIC_TxATT[0]
1363,Tx ATT　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_TxATT[1]
1364,Tx ATT　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_TxATT[2]
1365,Tx ATT　レジスタ設定値,0x00,0xFF,－－－,0x40,0x40,VND_RFIC_TxATT[3]
1366,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x0E,0x0E,VND_RFIC_RSSI[0]
1367,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[1]
1368,RSSI　レジスタ設定値,0x00,0xFF,－－－,0xFF,0xFF,VND_RFIC_RSSI[2]
1369,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[3]
1370,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[4]
1371,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[5]
1372,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[6]
1373,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x00,0x00,VND_RFIC_RSSI[7]
1374,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x0D,0x0D,VND_RFIC_RSSI[8]
1375,RSSI　レジスタ設定値,0x00,0xFF,－－－,0x67,0x67,VND_RFIC_RSSI[9]
1376,Power Up　レジスタ設定値,0x00,0xFF,－－－,0x3C,0x3C,VND_RFIC_PowerUp[0]
1377,Power Up　レジスタ設定値,0x00,0xFF,－－－,0x21,0x21,VND_RFIC_PowerUp[1]
1378,アップリンクコード相関検出しきい値(プリアンブル),0x0000,0xFFFF,,--,0x2000,
1379,アップリンクコード相関検出しきい値(データ),0x0000,0xFFFF,,--,0x2000,
1380,アップリンク相関演算開始しきい値,0x0000,0xFFFF,,--,0x1000,
1381,アップリンク相関演算開始タイミング,0x00000,0x1FFFF,,--,0x00000,
1382,アップリンクTDMA補正値（RF+モデム受信処理遅延量：⊿2）,0x00,0xFF,,0x0C,--,
1383,アップリンクTDMA割り込みタイミング補正量（受信割り込み調整値：α）,0x00,0x7F,,0x05,--,
1384,アップリンクTDMA割り込みタイミング補正量（RF+モデム送信処理遅延：β）,0x00,0xFF,,0x8A,--,
1385,ダウンリンク拡散コード0～7,0x0000000000000000,0x3FFFFFFFFFFFFFFF,,0x3EACDDA4E2F28C20,0x3EACDDA4E2F28C20,
1386,ダウンリンクコード相関検出しきい値1(検出),0x000,0x3FF,,0x005,--,
1387,ダウンリンクコード相関検出しきい値2(未検出),0x000,0x3FF,,0x005,--,
1388,ダウンリンクコード相関検出保護段数,0x00,0x77,,0x20,--,
1389,ダウンリンクコード相関検出ウィンドウ幅,0x0,0x3,,0x2,--,
1390,ダウンリンク同期検出(相関検出)しきい値1(検出),0x00,0xFF,,0x0C,--,
1391,ダウンリンク同期検出(相関検出)しきい値2(未検出),0x00,0xFF,,0x0A,--,
1392,ダウンリンク同期コード,0x00000000,0xFFFFFFFF,,0x1ACFFC1D,0x1ACFFC1D,
1393,ダウンリンク同期検出保護段数,0x00,0x77,,0x22,--,
1394,AFCループゲイン,0x00,0x77,,0x15,--,
1395,初期周波数推定時間,0x0,0x3,,0x3,--,
1396,初期周波数推定ゲイン,0x0000,0xFFFF,,TBD,--,
1397,キャリア同期収束閾値,0x00000000,0xFFFFFFFF,,TBD,--,
1398,キャリア同期タイムアウト設定,0x00000000,0x80FFFFFF,,TBD,--,
1399,TCXO用オフセット,0x0000,0x3FFF,,0x2000,--,
1400,キャリア同期保護段数,0x00,0xFF,,0x7F,--,
1401,AGC手動モード（On/Off）,0x0,0x1,,0x0,--,
1402,AGC 設定ゲイン初期値,0x00,0x1F,,0x1B,--,
1403,AGC　Manualモード時設定値,0x00,0x1F,,0x1B,--,
1404,AGC　ゲイン変更設定Step,0x0,0x3,,0x00,--,
1405,AGC収束目標値(振幅単位),0x00000000,0xFFFFFFFF,,0x000045B1,--,
1406,AGC 収束目標上限範囲,0x00000000,0xFFFFFFFF,,0x5092B50D,--,
1407,AGC 収束目標下限範囲,0x00000000,0xFFFFFFFF,,0x32D5F9A5,--,
1408,AGC UnLock上限範囲,0x00000000,0xFFFFFFFF,,0x657014EE,--,
1409,AGC UnLock下限範囲,0x00000000,0xFFFFFFFF,,0x28612177,--,
1410,AGC 無信号状態判定閾値,0x00000000,0xFFFFFFFF,,0x2012E2EA,--,
1411,AGC積分チップ数,0x0,0x3,,0x0,--,
1412,AGCウェイト時間(［ｕｓ］),0x00,0xFF,,0x64,--,
1413,ALC手動モード（On/Off）,0x0,0x1,,0x0,--,
1414,ALC送信パワー取り込み周期,0x00,0x7F,,0x00,--,
1415,ALC送信パワー平均算出サンプル数,0x0,0xF,,0x6,--,
1416,ALC補正ループ係数,0x0,0x3,,0x2,--,
1417,ALC送信パワー上限値,0x000,0x3FF,,0x135,--,
1418,ALC補正値初期値（手動時設定値）,0x000,0x3FF,,0x01F,--,
1419,ALC送信パワー収束上限閾値,0x000,0x3FF,,0x0C3,--,
1420,ALC送信パワー収束下限閾値,0x000,0x3FF,,0x0B6,--,
1421,ALC用TxATT安定Wait時間,0x0,0xF,,0x2,--,
1422,Turbo復号Iteration数,0x00,0x1F,,0x13,--,
1424,TCXO　初期値,0x0000,0x3FFF,hex,0x1FFF,0x1FFF,VND_TCXO_DefaultVal
1425,RSSIオフセット,-30000,30000,0.01dB,-6217,-6217,VND_RSSIoffset
1426,RF Synthesizer　設定待ち時間,1,1000,ms,1,1,VND_RFPLL_WaitTime_0
1427,RF Synthesizer　設定待ち時間,1,1000,us,960,960,VND_RFPLL_WaitTime_1
1428,RF Synthesizer　設定待ち時間,1,1000,us,960,960,VND_RFPLL_WaitTime_2
1429,RF TxPLL Lock 待ち時間,1,1000,us,1000,1000,VND_TXPLL_LocktTime
1430,AuxADCでの温度測定。温度変換係数,0,100,℃/LSB,87,87,VND_RFIC_TempSlope
1431,AuxADCでの温度測定時の待ち時間,0,500,ms,10,10,VND_RFIC_TempWait
1432,Tx Quad Cal wait回数,1,100,－－－,36,36,VND_TxQuad_CalCount
1433,Dummy write後の待機時間,0,1000,us,10,10,VND_RFIC_WaitTime
1434,General Wait時間,0,100,ms,20,20,VND_GENERAL_WaitTime_0
1435,BB PLL Wait時間,0,1000,us,700,700,VND_BBPLLl_WaitTime_0
1436,Tx BB Filter Wait時間,0,1000,us,130,20,VND_TxBBFil_WaitTime_0
1437,Rx BB Filter Wait時間,0,1000,us,30,250,VND_RxBBFil_WaitTime_0
1438,RF Synthesizer　ロック待ち時間,0,1000,ms,20,20,VND_RFIC_RfSynthLockWaitTime
1439,POWER周波数補正テーブル(0ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[0]
1440,POWER周波数補正テーブル(1ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[1]
1441,POWER周波数補正テーブル(2ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[2]
1442,POWER周波数補正テーブル(3ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[3]
1443,POWER周波数補正テーブル(4ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[4]
1444,POWER周波数補正テーブル(5ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[5]
1445,POWER周波数補正テーブル(6ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[6]
1446,POWER周波数補正テーブル(7ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[7]
1447,POWER周波数補正テーブル(8ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[8]
1448,POWER周波数補正テーブル(9ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[9]
1449,POWER周波数補正テーブル(10ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[10]
1450,POWER周波数補正テーブル(11ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[11]
1451,POWER周波数補正テーブル(12ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[12]
1452,POWER周波数補正テーブル(13ch),-60,60,0.05dB/dec,0,0,TxALCFreq_Revision[13]
1453,RSSI温度補正テーブル(0),-2000,2000,0.01dB,-378,-378,RSSITemp_Revision[0]
1454,RSSI温度補正テーブル(1),-2000,2000,0.01dB,-374,-374,RSSITemp_Revision[1]
1455,RSSI温度補正テーブル(2),-2000,2000,0.01dB,-370,-370,RSSITemp_Revision[2]
1456,RSSI温度補正テーブル(3),-2000,2000,0.01dB,-366,-366,RSSITemp_Revision[3]
1457,RSSI温度補正テーブル(4),-2000,2000,0.01dB,-362,-362,RSSITemp_Revision[4]
1458,RSSI温度補正テーブル(5),-2000,2000,0.01dB,-359,-359,RSSITemp_Revision[5]
1459,RSSI温度補正テーブル(6),-2000,2000,0.01dB,-355,-355,RSSITemp_Revision[6]
1460,RSSI温度補正テーブル(7),-2000,2000,0.01dB,-351,-351,RSSITemp_Revision[7]
1461,RSSI温度補正テーブル(8),-2000,2000,0.01dB,-347,-347,RSSITemp_Revision[8]
1462,RSSI温度補正テーブル(9),-2000,2000,0.01dB,-343,-343,RSSITemp_Revision[9]
1463,RSSI温度補正テーブル(10),-2000,2000,0.01dB,-339,-339,RSSITemp_Revision[10]
1464,RSSI温度補正テーブル(11),-2000,2000,0.01dB,-335,-335,RSSITemp_Revision[11]
1465,RSSI温度補正テーブル(12),-2000,2000,0.01dB,-332,-332,RSSITemp_Revision[12]
1466,RSSI温度補正テーブル(13),-2000,2000,0.01dB,-328,-328,RSSITemp_Revision[13]
1467,RSSI温度補正テーブル(14),-2000,2000,0.01dB,-324,-324,RSSITemp_Revision[14]
1468,RSSI温度補正テーブル(15),-2000,2000,0.01dB,-320,-320,RSSITemp_Revision[15]
1469,RSSI温度補正テーブル(16),-2000,2000,0.01dB,-316,-316,RSSITemp_Revision[16]
1470,RSSI温度補正テーブル(17),-2000,2000,0.01dB,-312,-312,RSSITemp_Revision[17]
1471,RSSI温度補正テーブル(18),-2000,2000,0.01dB,-308,-308,RSSITemp_Revision[18]
1472,RSSI温度補正テーブル(19),-2000,2000,0.01dB,-305,-305,RSSITemp_Revision[19]
1473,RSSI温度補正テーブル(20),-2000,2000,0.01dB,-301,-301,RSSITemp_Revision[20]
1474,RSSI温度補正テーブル(21),-2000,2000,0.01dB,-297,-297,RSSITemp_Revision[21]
1475,RSSI温度補正テーブル(22),-2000,2000,0.01dB,-293,-293,RSSITemp_Revision[22]
1476,RSSI温度補正テーブル(23),-2000,2000,0.01dB,-289,-289,RSSITemp_Revision[23]
1477,RSSI温度補正テーブル(24),-2000,2000,0.01dB,-285,-285,RSSITemp_Revision[24]
1478,RSSI温度補正テーブル(25),-2000,2000,0.01dB,-281,-281,RSSITemp_Revision[25]
1479,RSSI温度補正テーブル(26),-2000,2000,0.01dB,-277,-277,RSSITemp_Revision[26]
1480,RSSI温度補正テーブル(27),-2000,2000,0.01dB,-274,-274,RSSITemp_Revision[27]
1481,RSSI温度補正テーブル(28),-2000,2000,0.01dB,-270,-270,RSSITemp_Revision[28]
1482,RSSI温度補正テーブル(29),-2000,2000,0.01dB,-266,-266,RSSITemp_Revision[29]
1483,RSSI温度補正テーブル(30),-2000,2000,0.01dB,-262,-262,RSSITemp_Revision[30]
1484,RSSI温度補正テーブル(31),-2000,2000,0.01dB,-258,-258,RSSITemp_Revision[31]
1485,RSSI温度補正テーブル(32),-2000,2000,0.01dB,-254,-254,RSSITemp_Revision[32]
1486,RSSI温度補正テーブル(33),-2000,2000,0.01dB,-250,-250,RSSITemp_Revision[33]
1487,RSSI温度補正テーブル(34),-2000,2000,0.01dB,-247,-247,RSSITemp_Revision[34]
1488,RSSI温度補正テーブル(35),-2000,2000,0.01dB,-243,-243,RSSITemp_Revision[35]
1489,RSSI温度補正テーブル(36),-2000,2000,0.01dB,-239,-239,RSSITemp_Revision[36]
1490,RSSI温度補正テーブル(37),-2000,2000,0.01dB,-235,-235,RSSITemp_Revision[37]
1491,RSSI温度補正テーブル(38),-2000,2000,0.01dB,-231,-231,RSSITemp_Revision[38]
1492,RSSI温度補正テーブル(39),-2000,2000,0.01dB,-227,-227,RSSITemp_Revision[39]
1493,RSSI温度補正テーブル(40),-2000,2000,0.01dB,-223,-223,RSSITemp_Revision[40]
1494,RSSI温度補正テーブル(41),-2000,2000,0.01dB,-220,-220,RSSITemp_Revision[41]
1495,RSSI温度補正テーブル(42),-2000,2000,0.01dB,-216,-216,RSSITemp_Revision[42]
1496,RSSI温度補正テーブル(43),-2000,2000,0.01dB,-212,-212,RSSITemp_Revision[43]
1497,RSSI温度補正テーブル(44),-2000,2000,0.01dB,-208,-208,RSSITemp_Revision[44]
1498,RSSI温度補正テーブル(45),-2000,2000,0.01dB,-204,-204,RSSITemp_Revision[45]
1499,RSSI温度補正テーブル(46),-2000,2000,0.01dB,-200,-200,RSSITemp_Revision[46]
1500,RSSI温度補正テーブル(47),-2000,2000,0.01dB,-196,-196,RSSITemp_Revision[47]
1501,RSSI温度補正テーブル(48),-2000,2000,0.01dB,-192,-192,RSSITemp_Revision[48]
1502,RSSI温度補正テーブル(49),-2000,2000,0.01dB,-189,-189,RSSITemp_Revision[49]
1503,RSSI温度補正テーブル(50),-2000,2000,0.01dB,-185,-185,RSSITemp_Revision[50]
1504,RSSI温度補正テーブル(51),-2000,2000,0.01dB,-181,-181,RSSITemp_Revision[51]
1505,RSSI温度補正テーブル(52),-2000,2000,0.01dB,-177,-177,RSSITemp_Revision[52]
1506,RSSI温度補正テーブル(53),-2000,2000,0.01dB,-173,-173,RSSITemp_Revision[53]
1507,RSSI温度補正テーブル(54),-2000,2000,0.01dB,-169,-169,RSSITemp_Revision[54]
1508,RSSI温度補正テーブル(55),-2000,2000,0.01dB,-165,-165,RSSITemp_Revision[55]
1509,RSSI温度補正テーブル(56),-2000,2000,0.01dB,-162,-162,RSSITemp_Revision[56]
1510,RSSI温度補正テーブル(57),-2000,2000,0.01dB,-158,-158,RSSITemp_Revision[57]
1511,RSSI温度補正テーブル(58),-2000,2000,0.01dB,-154,-154,RSSITemp_Revision[58]
1512,RSSI温度補正テーブル(59),-2000,2000,0.01dB,-150,-150,RSSITemp_Revision[59]
1513,RSSI温度補正テーブル(60),-2000,2000,0.01dB,-146,-146,RSSITemp_Revision[60]
1514,RSSI温度補正テーブル(61),-2000,2000,0.01dB,-142,-142,RSSITemp_Revision[61]
1515,RSSI温度補正テーブル(62),-2000,2000,0.01dB,-138,-138,RSSITemp_Revision[62]
1516,RSSI温度補正テーブル(63),-2000,2000,0.01dB,-135,-135,RSSITemp_Revision[63]
1517,RSSI温度補正テーブル(64),-2000,2000,0.01dB,-131,-131,RSSITemp_Revision[64]
1518,RSSI温度補正テーブル(65),-2000,2000,0.01dB,-127,-127,RSSITemp_Revision[65]
1519,RSSI温度補正テーブル(66),-2000,2000,0.01dB,-123,-123,RSSITemp_Revision[66]
1520,RSSI温度補正テーブル(67),-2000,2000,0.01dB,-119,-119,RSSITemp_Revision[67]
1521,RSSI温度補正テーブル(68),-2000,2000,0.01dB,-115,-115,RSSITemp_Revision[68]
1522,RSSI温度補正テーブル(69),-2000,2000,0.01dB,-111,-111,RSSITemp_Revision[69]
1523,RSSI温度補正テーブル(70),-2000,2000,0.01dB,-108,-108,RSSITemp_Revision[70]
1524,RSSI温度補正テーブル(71),-2000,2000,0.01dB,-104,-104,RSSITemp_Revision[71]
1525,RSSI温度補正テーブル(72),-2000,2000,0.01dB,-100,-100,RSSITemp_Revision[72]
1526,RSSI温度補正テーブル(73),-2000,2000,0.01dB,-96,-96,RSSITemp_Revision[73]
1527,RSSI温度補正テーブル(74),-2000,2000,0.01dB,-92,-92,RSSITemp_Revision[74]
1528,RSSI温度補正テーブル(75),-2000,2000,0.01dB,-88,-88,RSSITemp_Revision[75]
1529,RSSI温度補正テーブル(76),-2000,2000,0.01dB,-84,-84,RSSITemp_Revision[76]
1530,RSSI温度補正テーブル(77),-2000,2000,0.01dB,-80,-80,RSSITemp_Revision[77]
1531,RSSI温度補正テーブル(78),-2000,2000,0.01dB,-77,-77,RSSITemp_Revision[78]
1532,RSSI温度補正テーブル(79),-2000,2000,0.01dB,-73,-73,RSSITemp_Revision[79]
1533,RSSI温度補正テーブル(80),-2000,2000,0.01dB,-69,-69,RSSITemp_Revision[80]
1534,RSSI温度補正テーブル(81),-2000,2000,0.01dB,-65,-65,RSSITemp_Revision[81]
1535,RSSI温度補正テーブル(82),-2000,2000,0.01dB,-61,-61,RSSITemp_Revision[82]
1536,RSSI温度補正テーブル(83),-2000,2000,0.01dB,-57,-57,RSSITemp_Revision[83]
1537,RSSI温度補正テーブル(84),-2000,2000,0.01dB,-53,-53,RSSITemp_Revision[84]
1538,RSSI温度補正テーブル(85),-2000,2000,0.01dB,-50,-50,RSSITemp_Revision[85]
1539,RSSI温度補正テーブル(86),-2000,2000,0.01dB,-46,-46,RSSITemp_Revision[86]
1540,RSSI温度補正テーブル(87),-2000,2000,0.01dB,-42,-42,RSSITemp_Revision[87]
1541,RSSI温度補正テーブル(88),-2000,2000,0.01dB,-38,-38,RSSITemp_Revision[88]
1542,RSSI温度補正テーブル(89),-2000,2000,0.01dB,-34,-34,RSSITemp_Revision[89]
1543,RSSI温度補正テーブル(90),-2000,2000,0.01dB,-30,-30,RSSITemp_Revision[90]
1544,RSSI温度補正テーブル(91),-2000,2000,0.01dB,-26,-26,RSSITemp_Revision[91]
1545,RSSI温度補正テーブル(92),-2000,2000,0.01dB,-23,-23,RSSITemp_Revision[92]
1546,RSSI温度補正テーブル(93),-2000,2000,0.01dB,-19,-19,RSSITemp_Revision[93]
1547,RSSI温度補正テーブル(94),-2000,2000,0.01dB,-15,-15,RSSITemp_Revision[94]
1548,RSSI温度補正テーブル(95),-2000,2000,0.01dB,-11,-11,RSSITemp_Revision[95]
1549,RSSI温度補正テーブル(96),-2000,2000,0.01dB,-7,-7,RSSITemp_Revision[96]
1550,RSSI温度補正テーブル(97),-2000,2000,0.01dB,-3,-3,RSSITemp_Revision[97]
1551,RSSI温度補正テーブル(98),-2000,2000,0.01dB,0,0,RSSITemp_Revision[98]
1552,RSSI温度補正テーブル(99),-2000,2000,0.01dB,4,4,RSSITemp_Revision[99]
1553,RSSI温度補正テーブル(100),-2000,2000,0.01dB,7,7,RSSITemp_Revision[100]
1554,RSSI温度補正テーブル(101),-2000,2000,0.01dB,10,10,RSSITemp_Revision[101]
1555,RSSI温度補正テーブル(102),-2000,2000,0.01dB,13,13,RSSITemp_Revision[102]
1556,RSSI温度補正テーブル(103),-2000,2000,0.01dB,16,16,RSSITemp_Revision[103]
1557,RSSI温度補正テーブル(104),-2000,2000,0.01dB,19,19,RSSITemp_Revision[104]
1558,RSSI温度補正テーブル(105),-2000,2000,0.01dB,23,23,RSSITemp_Revision[105]
1559,RSSI温度補正テーブル(106),-2000,2000,0.01dB,26,26,RSSITemp_Revision[106]
1560,RSSI温度補正テーブル(107),-2000,2000,0.01dB,29,29,RSSITemp_Revision[107]
1561,RSSI温度補正テーブル(108),-2000,2000,0.01dB,32,32,RSSITemp_Revision[108]
1562,RSSI温度補正テーブル(109),-2000,2000,0.01dB,35,35,RSSITemp_Revision[109]
1563,RSSI温度補正テーブル(110),-2000,2000,0.01dB,38,38,RSSITemp_Revision[110]
1564,RSSI温度補正テーブル(111),-2000,2000,0.01dB,42,42,RSSITemp_Revision[111]
1565,RSSI温度補正テーブル(112),-2000,2000,0.01dB,45,45,RSSITemp_Revision[112]
1566,RSSI温度補正テーブル(113),-2000,2000,0.01dB,48,48,RSSITemp_Revision[113]
1567,RSSI温度補正テーブル(114),-2000,2000,0.01dB,51,51,RSSITemp_Revision[114]
1568,RSSI温度補正テーブル(115),-2000,2000,0.01dB,54,54,RSSITemp_Revision[115]
1569,RSSI温度補正テーブル(116),-2000,2000,0.01dB,57,57,RSSITemp_Revision[116]
1570,RSSI温度補正テーブル(117),-2000,2000,0.01dB,61,61,RSSITemp_Revision[117]
1571,RSSI温度補正テーブル(118),-2000,2000,0.01dB,64,64,RSSITemp_Revision[118]
1572,RSSI温度補正テーブル(119),-2000,2000,0.01dB,67,67,RSSITemp_Revision[119]
1573,RSSI温度補正テーブル(120),-2000,2000,0.01dB,70,70,RSSITemp_Revision[120]
1574,RSSI温度補正テーブル(121),-2000,2000,0.01dB,73,73,RSSITemp_Revision[121]
1575,RSSI温度補正テーブル(122),-2000,2000,0.01dB,76,76,RSSITemp_Revision[122]
1576,RSSI温度補正テーブル(123),-2000,2000,0.01dB,80,80,RSSITemp_Revision[123]
1577,RSSI温度補正テーブル(124),-2000,2000,0.01dB,83,83,RSSITemp_Revision[124]
1578,RSSI温度補正テーブル(125),-2000,2000,0.01dB,86,86,RSSITemp_Revision[125]
1579,RSSI温度補正テーブル(126),-2000,2000,0.01dB,89,89,RSSITemp_Revision[126]
1580,RSSI温度補正テーブル(127),-2000,2000,0.01dB,92,92,RSSITemp_Revision[127]
1581,RSSI温度補正テーブル(128),-2000,2000,0.01dB,95,95,RSSITemp_Revision[128]
1582,RSSI温度補正テーブル(129),-2000,2000,0.01dB,99,99,RSSITemp_Revision[129]
1583,RSSI温度補正テーブル(130),-2000,2000,0.01dB,102,102,RSSITemp_Revision[130]
1584,RSSI温度補正テーブル(131),-2000,2000,0.01dB,105,105,RSSITemp_Revision[131]
1585,RSSI温度補正テーブル(132),-2000,2000,0.01dB,108,108,RSSITemp_Revision[132]
1586,RSSI温度補正テーブル(133),-2000,2000,0.01dB,111,111,RSSITemp_Revision[133]
1587,RSSI温度補正テーブル(134),-2000,2000,0.01dB,114,114,RSSITemp_Revision[134]
1588,RSSI温度補正テーブル(135),-2000,2000,0.01dB,118,118,RSSITemp_Revision[135]
1589,RSSI温度補正テーブル(136),-2000,2000,0.01dB,121,121,RSSITemp_Revision[136]
1590,RSSI温度補正テーブル(137),-2000,2000,0.01dB,124,124,RSSITemp_Revision[137]
1591,RSSI温度補正テーブル(138),-2000,2000,0.01dB,127,127,RSSITemp_Revision[138]
1592,RSSI温度補正テーブル(139),-2000,2000,0.01dB,130,130,RSSITemp_Revision[139]
1593,RSSI温度補正テーブル(140),-2000,2000,0.01dB,133,133,RSSITemp_Revision[140]
1594,RSSI温度補正テーブル(141),-2000,2000,0.01dB,137,137,RSSITemp_Revision[141]
1595,RSSI温度補正テーブル(142),-2000,2000,0.01dB,140,140,RSSITemp_Revision[142]
1596,RSSI温度補正テーブル(143),-2000,2000,0.01dB,143,143,RSSITemp_Revision[143]
1597,RSSI温度補正テーブル(144),-2000,2000,0.01dB,146,146,RSSITemp_Revision[144]
1598,RSSI温度補正テーブル(145),-2000,2000,0.01dB,149,149,RSSITemp_Revision[145]
1599,RSSI温度補正テーブル(146),-2000,2000,0.01dB,152,152,RSSITemp_Revision[146]
1600,RSSI温度補正テーブル(147),-2000,2000,0.01dB,156,156,RSSITemp_Revision[147]
1601,RSSI温度補正テーブル(148),-2000,2000,0.01dB,159,159,RSSITemp_Revision[148]
1602,RSSI温度補正テーブル(149),-2000,2000,0.01dB,162,162,RSSITemp_Revision[149]
1603,RSSI温度補正テーブル(150),-2000,2000,0.01dB,165,165,RSSITemp_Revision[150]
1604,RSSI温度補正テーブル(151),-2000,2000,0.01dB,168,168,RSSITemp_Revision[151]
1605,RSSI温度補正テーブル(152),-2000,2000,0.01dB,171,171,RSSITemp_Revision[152]
1606,RSSI温度補正テーブル(153),-2000,2000,0.01dB,175,175,RSSITemp_Revision[153]
1607,RSSI温度補正テーブル(154),-2000,2000,0.01dB,178,178,RSSITemp_Revision[154]
1608,RSSI温度補正テーブル(155),-2000,2000,0.01dB,181,181,RSSITemp_Revision[155]
1609,RSSI温度補正テーブル(156),-2000,2000,0.01dB,184,184,RSSITemp_Revision[156]
1610,RSSI温度補正テーブル(157),-2000,2000,0.01dB,187,187,RSSITemp_Revision[157]
1611,RSSI温度補正テーブル(158),-2000,2000,0.01dB,190,190,RSSITemp_Revision[158]
1612,RSSI温度補正テーブル(159),-2000,2000,0.01dB,194,194,RSSITemp_Revision[159]
1613,RSSI温度補正テーブル(160),-2000,2000,0.01dB,197,197,RSSITemp_Revision[160]
1614,RSSI温度補正テーブル(161),-2000,2000,0.01dB,200,200,RSSITemp_Revision[161]
1615,RSSI温度補正テーブル(162),-2000,2000,0.01dB,203,203,RSSITemp_Revision[162]
1616,RSSI温度補正テーブル(163),-2000,2000,0.01dB,206,206,RSSITemp_Revision[163]
1617,RSSI温度補正テーブル(164),-2000,2000,0.01dB,209,209,RSSITemp_Revision[164]
1618,RSSI温度補正テーブル(165),-2000,2000,0.01dB,213,213,RSSITemp_Revision[165]
1619,RSSI温度補正テーブル(166),-2000,2000,0.01dB,216,216,RSSITemp_Revision[166]
1620,RSSI温度補正テーブル(167),-2000,2000,0.01dB,219,219,RSSITemp_Revision[167]
1621,RSSI温度補正テーブル(168),-2000,2000,0.01dB,222,222,RSSITemp_Revision[168]
1622,RSSI温度補正テーブル(169),-2000,2000,0.01dB,225,225,RSSITemp_Revision[169]
1623,RSSI温度補正テーブル(170),-2000,2000,0.01dB,228,228,RSSITemp_Revision[170]
1624,RSSI温度補正テーブル(171),-2000,2000,0.01dB,232,232,RSSITemp_Revision[171]
1625,RSSI温度補正テーブル(172),-2000,2000,0.01dB,235,235,RSSITemp_Revision[172]
1626,RSSI温度補正テーブル(173),-2000,2000,0.01dB,238,238,RSSITemp_Revision[173]
1627,RSSI温度補正テーブル(174),-2000,2000,0.01dB,241,241,RSSITemp_Revision[174]
1628,RSSI温度補正テーブル(175),-2000,2000,0.01dB,244,244,RSSITemp_Revision[175]
1629,RSSI温度補正テーブル(176),-2000,2000,0.01dB,247,247,RSSITemp_Revision[176]
1630,RSSI温度補正テーブル(177),-2000,2000,0.01dB,251,251,RSSITemp_Revision[177]
1631,RSSI温度補正テーブル(178),-2000,2000,0.01dB,254,254,RSSITemp_Revision[178]
1632,RSSI温度補正テーブル(179),-2000,2000,0.01dB,257,257,RSSITemp_Revision[179]
1633,RSSI温度補正テーブル(180),-2000,2000,0.01dB,260,260,RSSITemp_Revision[180]
1634,RSSI温度補正テーブル(181),-2000,2000,0.01dB,263,263,RSSITemp_Revision[181]
1635,RSSI温度補正テーブル(182),-2000,2000,0.01dB,266,266,RSSITemp_Revision[182]
1636,RSSI温度補正テーブル(183),-2000,2000,0.01dB,270,270,RSSITemp_Revision[183]
1637,RSSI温度補正テーブル(184),-2000,2000,0.01dB,273,273,RSSITemp_Revision[184]
1638,RSSI温度補正テーブル(185),-2000,2000,0.01dB,276,276,RSSITemp_Revision[185]
1639,RSSI温度補正テーブル(186),-2000,2000,0.01dB,279,279,RSSITemp_Revision[186]
1640,RSSI温度補正テーブル(187),-2000,2000,0.01dB,282,282,RSSITemp_Revision[187]
1641,RSSI温度補正テーブル(188),-2000,2000,0.01dB,285,285,RSSITemp_Revision[188]
1642,RSSI温度補正テーブル(189),-2000,2000,0.01dB,289,289,RSSITemp_Revision[189]
1643,RSSI温度補正テーブル(190),-2000,2000,0.01dB,292,292,RSSITemp_Revision[190]
1644,RSSI温度補正テーブル(191),-2000,2000,0.01dB,295,295,RSSITemp_Revision[191]
1645,RSSI温度補正テーブル(192),-2000,2000,0.01dB,298,298,RSSITemp_Revision[192]
1646,RSSI温度補正テーブル(193),-2000,2000,0.01dB,301,301,RSSITemp_Revision[193]
1647,RSSI温度補正テーブル(194),-2000,2000,0.01dB,304,304,RSSITemp_Revision[194]
1648,RSSI温度補正テーブル(195),-2000,2000,0.01dB,308,308,RSSITemp_Revision[195]
1649,RSSI温度補正テーブル(196),-2000,2000,0.01dB,311,311,RSSITemp_Revision[196]
1650,RSSI温度補正テーブル(197),-2000,2000,0.01dB,314,314,RSSITemp_Revision[197]
1651,RSSI温度補正テーブル(198),-2000,2000,0.01dB,317,317,RSSITemp_Revision[198]
1652,RSSI温度補正テーブル(199),-2000,2000,0.01dB,320,320,RSSITemp_Revision[199]
1653,RSSI温度補正テーブル(200),-2000,2000,0.01dB,323,323,RSSITemp_Revision[200]
1654,RSSI温度補正テーブル(201),-2000,2000,0.01dB,327,327,RSSITemp_Revision[201]
1655,RSSI温度補正テーブル(202),-2000,2000,0.01dB,330,330,RSSITemp_Revision[202]
1656,RSSI温度補正テーブル(203),-2000,2000,0.01dB,333,333,RSSITemp_Revision[203]
1657,RSSI温度補正テーブル(204),-2000,2000,0.01dB,336,336,RSSITemp_Revision[204]
1658,RSSI温度補正テーブル(205),-2000,2000,0.01dB,339,339,RSSITemp_Revision[205]
1659,RSSI温度補正テーブル(206),-2000,2000,0.01dB,342,342,RSSITemp_Revision[206]
1660,RSSI温度補正テーブル(207),-2000,2000,0.01dB,346,346,RSSITemp_Revision[207]
1661,RSSI温度補正テーブル(208),-2000,2000,0.01dB,349,349,RSSITemp_Revision[208]
1662,RSSI温度補正テーブル(209),-2000,2000,0.01dB,352,352,RSSITemp_Revision[209]
1663,RSSI温度補正テーブル(210),-2000,2000,0.01dB,355,355,RSSITemp_Revision[210]
1664,RSSI温度補正テーブル(211),-2000,2000,0.01dB,358,358,RSSITemp_Revision[211]
1665,RSSI温度補正テーブル(212),-2000,2000,0.01dB,361,361,RSSITemp_Revision[212]
1666,RSSI温度補正テーブル(213),-2000,2000,0.01dB,365,365,RSSITemp_Revision[213]
1667,RSSI温度補正テーブル(214),-2000,2000,0.01dB,368,368,RSSITemp_Revision[214]
1668,RSSI温度補正テーブル(215),-2000,2000,0.01dB,371,371,RSSITemp_Revision[215]
1669,RSSI温度補正テーブル(216),-2000,2000,0.01dB,374,374,RSSITemp_Revision[216]
1670,RSSI温度補正テーブル(217),-2000,2000,0.01dB,377,377,RSSITemp_Revision[217]
1671,RSSI温度補正テーブル(218),-2000,2000,0.01dB,380,380,RSSITemp_Revision[218]
1672,RSSI温度補正テーブル(219),-2000,2000,0.01dB,384,384,RSSITemp_Revision[219]
1673,RSSI温度補正テーブル(220),-2000,2000,0.01dB,387,387,RSSITemp_Revision[220]
1674,RSSI温度補正テーブル(221),-2000,2000,0.01dB,390,390,RSSITemp_Revision[221]
1675,RSSI温度補正テーブル(222),-2000,2000,0.01dB,393,393,RSSITemp_Revision[222]
1676,RSSI温度補正テーブル(223),-2000,2000,0.01dB,396,396,RSSITemp_Revision[223]
1677,RSSI温度補正テーブル(224),-2000,2000,0.01dB,399,399,RSSITemp_Revision[224]
1678,RSSI温度補正テーブル(225),-2000,2000,0.01dB,403,403,RSSITemp_Revision[225]
1679,RSSI温度補正テーブル(226),-2000,2000,0.01dB,406,406,RSSITemp_Revision[226]
1680,RSSI温度補正テーブル(227),-2000,2000,0.01dB,409,409,RSSITemp_Revision[227]
1681,RSSI温度補正テーブル(228),-2000,2000,0.01dB,412,412,RSSITemp_Revision[228]
1682,RSSI温度補正テーブル(229),-2000,2000,0.01dB,415,415,RSSITemp_Revision[229]
1683,RSSI温度補正テーブル(230),-2000,2000,0.01dB,418,418,RSSITemp_Revision[230]
1684,RSSI温度補正テーブル(231),-2000,2000,0.01dB,422,422,RSSITemp_Revision[231]
1685,RSSI温度補正テーブル(232),-2000,2000,0.01dB,425,425,RSSITemp_Revision[232]
1686,RSSI温度補正テーブル(233),-2000,2000,0.01dB,428,428,RSSITemp_Revision[233]
1687,RSSI温度補正テーブル(234),-2000,2000,0.01dB,431,431,RSSITemp_Revision[234]
1688,RSSI温度補正テーブル(235),-2000,2000,0.01dB,434,434,RSSITemp_Revision[235]
1689,RSSI温度補正テーブル(236),-2000,2000,0.01dB,437,437,RSSITemp_Revision[236]
1690,RSSI温度補正テーブル(237),-2000,2000,0.01dB,441,441,RSSITemp_Revision[237]
1691,RSSI温度補正テーブル(238),-2000,2000,0.01dB,444,444,RSSITemp_Revision[238]
1692,RSSI温度補正テーブル(239),-2000,2000,0.01dB,447,447,RSSITemp_Revision[239]
1693,RSSI温度補正テーブル(240),-2000,2000,0.01dB,450,450,RSSITemp_Revision[240]
1694,RSSI温度補正テーブル(241),-2000,2000,0.01dB,453,453,RSSITemp_Revision[241]
1695,RSSI温度補正テーブル(242),-2000,2000,0.01dB,456,456,RSSITemp_Revision[242]
1696,RSSI温度補正テーブル(243),-2000,2000,0.01dB,460,460,RSSITemp_Revision[243]
1697,RSSI温度補正テーブル(244),-2000,2000,0.01dB,463,463,RSSITemp_Revision[244]
1698,RSSI温度補正テーブル(245),-2000,2000,0.01dB,466,466,RSSITemp_Revision[245]
1699,RSSI温度補正テーブル(246),-2000,2000,0.01dB,469,469,RSSITemp_Revision[246]
1700,RSSI温度補正テーブル(247),-2000,2000,0.01dB,472,472,RSSITemp_Revision[247]
1701,RSSI温度補正テーブル(248),-2000,2000,0.01dB,475,475,RSSITemp_Revision[248]
1702,RSSI温度補正テーブル(249),-2000,2000,0.01dB,479,479,RSSITemp_Revision[249]
1703,RSSI温度補正テーブル(250),-2000,2000,0.01dB,482,482,RSSITemp_Revision[250]
1704,RSSI温度補正テーブル(251),-2000,2000,0.01dB,485,485,RSSITemp_Revision[251]
1705,RSSI温度補正テーブル(252),-2000,2000,0.01dB,488,488,RSSITemp_Revision[252]
1706,RSSI温度補正テーブル(253),-2000,2000,0.01dB,491,491,RSSITemp_Revision[253]
1707,RSSI温度補正テーブル(254),-2000,2000,0.01dB,494,494,RSSITemp_Revision[254]
1708,RSSI温度補正テーブル(255),-2000,2000,0.01dB,498,498,RSSITemp_Revision[255]
1709,アップリンクデータ拡散コード0,0x00000000,0xFFFFFFFF,,0x0056E5E,0x0056E5E,UP_SS_LSFR_INIT_D-0
,アップリンクデータ拡散コード1,0x00000000,0xFFFFFFFF,,0x005A11E,0x005A11E,UP_SS_LSFR_INIT_D-1
,アップリンクデータ拡散コード2,0x00000000,0xFFFFFFFF,,0x00AA981,0x00AA981,UP_SS_LSFR_INIT_D-2
,アップリンクデータ拡散コード3,0x00000000,0xFFFFFFFF,,0x00AF181,0x00AF181,UP_SS_LSFR_INIT_D-3
,アップリンクデータ拡散コード4,0x00000000,0xFFFFFFFF,,0x015EFD0,0x015EFD0,UP_SS_LSFR_INIT_D-4
,アップリンクデータ拡散コード5,0x00000000,0xFFFFFFFF,,0x01B18ED,0x01B18ED,UP_SS_LSFR_INIT_D-5
,アップリンクデータ拡散コード6,0x00000000,0xFFFFFFFF,,0x01BE2C7,0x01BE2C7,UP_SS_LSFR_INIT_D-6
,アップリンクデータ拡散コード7,0x00000000,0xFFFFFFFF,,0x01FA017,0x01FA017,UP_SS_LSFR_INIT_D-7
,アップリンクデータ拡散コード8,0x00000000,0xFFFFFFFF,,0x02142CA,0x02142CA,UP_SS_LSFR_INIT_D-8
,アップリンクデータ拡散コード9,0x00000000,0xFFFFFFFF,,0x0230E0F,0x0230E0F,UP_SS_LSFR_INIT_D-9
,アップリンクデータ拡散コード10,0x00000000,0xFFFFFFFF,,0x02DFD0B,0x02DFD0B,UP_SS_LSFR_INIT_D-10
,アップリンクデータ拡散コード11,0x00000000,0xFFFFFFFF,,0x030583C,0x030583C,UP_SS_LSFR_INIT_D-11
,アップリンクデータ拡散コード12,0x00000000,0xFFFFFFFF,,0x03F8B6F,0x03F8B6F,UP_SS_LSFR_INIT_D-12
,アップリンクデータ拡散コード13,0x00000000,0xFFFFFFFF,,0x041A364,0x041A364,UP_SS_LSFR_INIT_D-13
,アップリンクデータ拡散コード14,0x00000000,0xFFFFFFFF,,0x048FCB6,0x048FCB6,UP_SS_LSFR_INIT_D-14
,アップリンクデータ拡散コード15,0x00000000,0xFFFFFFFF,,0x04CE74B,0x04CE74B,UP_SS_LSFR_INIT_D-15
,アップリンクデータ拡散コード16,0x00000000,0xFFFFFFFF,,0x0551C26,0x0551C26,UP_SS_LSFR_INIT_D-16
,アップリンクデータ拡散コード17,0x00000000,0xFFFFFFFF,,0x05B9CEF,0x05B9CEF,UP_SS_LSFR_INIT_D-17
,アップリンクデータ拡散コード18,0x00000000,0xFFFFFFFF,,0x05C94C0,0x05C94C0,UP_SS_LSFR_INIT_D-18
,アップリンクデータ拡散コード19,0x00000000,0xFFFFFFFF,,0x05D597A,0x05D597A,UP_SS_LSFR_INIT_D-19
,アップリンクデータ拡散コード20,0x00000000,0xFFFFFFFF,,0x05F8E0C,0x05F8E0C,UP_SS_LSFR_INIT_D-20
,アップリンクデータ拡散コード21,0x00000000,0xFFFFFFFF,,0x0632A13,0x0632A13,UP_SS_LSFR_INIT_D-21
,アップリンクデータ拡散コード22,0x00000000,0xFFFFFFFF,,0x067D6A5,0x067D6A5,UP_SS_LSFR_INIT_D-22
,アップリンクデータ拡散コード23,0x00000000,0xFFFFFFFF,,0x06D6C67,0x06D6C67,UP_SS_LSFR_INIT_D-23
,アップリンクデータ拡散コード24,0x00000000,0xFFFFFFFF,,0x0730FDD,0x0730FDD,UP_SS_LSFR_INIT_D-24
,アップリンクデータ拡散コード25,0x00000000,0xFFFFFFFF,,0x073D916,0x073D916,UP_SS_LSFR_INIT_D-25
,アップリンクデータ拡散コード26,0x00000000,0xFFFFFFFF,,0x0798135,0x0798135,UP_SS_LSFR_INIT_D-26
,アップリンクデータ拡散コード27,0x00000000,0xFFFFFFFF,,0x07C0D11,0x07C0D11,UP_SS_LSFR_INIT_D-27
,アップリンクデータ拡散コード28,0x00000000,0xFFFFFFFF,,0x07F3D6B,0x07F3D6B,UP_SS_LSFR_INIT_D-28
,アップリンクデータ拡散コード29,0x00000000,0xFFFFFFFF,,0x08EF648,0x08EF648,UP_SS_LSFR_INIT_D-29
,アップリンクデータ拡散コード30,0x00000000,0xFFFFFFFF,,0x094B208,0x094B208,UP_SS_LSFR_INIT_D-30
,アップリンクデータ拡散コード31,0x00000000,0xFFFFFFFF,,0x099846D,0x099846D,UP_SS_LSFR_INIT_D-31
,アップリンクデータ拡散コード32,0x00000000,0xFFFFFFFF,,0x0A289A5,0x0A289A5,UP_SS_LSFR_INIT_D-32
,アップリンクデータ拡散コード33,0x00000000,0xFFFFFFFF,,0x0A45D65,0x0A45D65,UP_SS_LSFR_INIT_D-33
,アップリンクデータ拡散コード34,0x00000000,0xFFFFFFFF,,0x0AA99E3,0x0AA99E3,UP_SS_LSFR_INIT_D-34
,アップリンクデータ拡散コード35,0x00000000,0xFFFFFFFF,,0x0B28CE7,0x0B28CE7,UP_SS_LSFR_INIT_D-35
,アップリンクデータ拡散コード36,0x00000000,0xFFFFFFFF,,0x0BE87CF,0x0BE87CF,UP_SS_LSFR_INIT_D-36
,アップリンクデータ拡散コード37,0x00000000,0xFFFFFFFF,,0x0BF4447,0x0BF4447,UP_SS_LSFR_INIT_D-37
,アップリンクデータ拡散コード38,0x00000000,0xFFFFFFFF,,0x0C5C8BF,0x0C5C8BF,UP_SS_LSFR_INIT_D-38
,アップリンクデータ拡散コード39,0x00000000,0xFFFFFFFF,,0x0C8A1FA,0x0C8A1FA,UP_SS_LSFR_INIT_D-39
,アップリンクデータ拡散コード40,0x00000000,0xFFFFFFFF,,0x0C8E554,0x0C8E554,UP_SS_LSFR_INIT_D-40
,アップリンクデータ拡散コード41,0x00000000,0xFFFFFFFF,,0x0CAF835,0x0CAF835,UP_SS_LSFR_INIT_D-41
,アップリンクデータ拡散コード42,0x00000000,0xFFFFFFFF,,0x0E07185,0x0E07185,UP_SS_LSFR_INIT_D-42
,アップリンクデータ拡散コード43,0x00000000,0xFFFFFFFF,,0x0E3491C,0x0E3491C,UP_SS_LSFR_INIT_D-43
,アップリンクデータ拡散コード44,0x00000000,0xFFFFFFFF,,0x0EBF355,0x0EBF355,UP_SS_LSFR_INIT_D-44
,アップリンクデータ拡散コード45,0x00000000,0xFFFFFFFF,,0x0F55F67,0x0F55F67,UP_SS_LSFR_INIT_D-45
,アップリンクデータ拡散コード46,0x00000000,0xFFFFFFFF,,0x0FBAFB9,0x0FBAFB9,UP_SS_LSFR_INIT_D-46
,アップリンクデータ拡散コード47,0x00000000,0xFFFFFFFF,,0x0FD566B,0x0FD566B,UP_SS_LSFR_INIT_D-47
,アップリンクデータ拡散コード48,0x00000000,0xFFFFFFFF,,0x10104B4,0x10104B4,UP_SS_LSFR_INIT_D-48
,アップリンクデータ拡散コード49,0x00000000,0xFFFFFFFF,,0x101B314,0x101B314,UP_SS_LSFR_INIT_D-49
,アップリンクデータ拡散コード50,0x00000000,0xFFFFFFFF,,0x10A8554,0x10A8554,UP_SS_LSFR_INIT_D-50
,アップリンクデータ拡散コード51,0x00000000,0xFFFFFFFF,,0x10E0CB4,0x10E0CB4,UP_SS_LSFR_INIT_D-51
,アップリンクデータ拡散コード52,0x00000000,0xFFFFFFFF,,0x111482B,0x111482B,UP_SS_LSFR_INIT_D-52
,アップリンクデータ拡散コード53,0x00000000,0xFFFFFFFF,,0x113D193,0x113D193,UP_SS_LSFR_INIT_D-53
,アップリンクデータ拡散コード54,0x00000000,0xFFFFFFFF,,0x11AD19C,0x11AD19C,UP_SS_LSFR_INIT_D-54
,アップリンクデータ拡散コード55,0x00000000,0xFFFFFFFF,,0x1241B67,0x1241B67,UP_SS_LSFR_INIT_D-55
,アップリンクデータ拡散コード56,0x00000000,0xFFFFFFFF,,0x126CD5D,0x126CD5D,UP_SS_LSFR_INIT_D-56
,アップリンクデータ拡散コード57,0x00000000,0xFFFFFFFF,,0x12ADA2D,0x12ADA2D,UP_SS_LSFR_INIT_D-57
,アップリンクデータ拡散コード58,0x00000000,0xFFFFFFFF,,0x12DDFCC,0x12DDFCC,UP_SS_LSFR_INIT_D-58
,アップリンクデータ拡散コード59,0x00000000,0xFFFFFFFF,,0x139E6F1,0x139E6F1,UP_SS_LSFR_INIT_D-59
,アップリンクデータ拡散コード60,0x00000000,0xFFFFFFFF,,0x13E6A0A,0x13E6A0A,UP_SS_LSFR_INIT_D-60
,アップリンクデータ拡散コード61,0x00000000,0xFFFFFFFF,,0x143C4A6,0x143C4A6,UP_SS_LSFR_INIT_D-61
,アップリンクデータ拡散コード62,0x00000000,0xFFFFFFFF,,0x1520AA7,0x1520AA7,UP_SS_LSFR_INIT_D-62
,アップリンクデータ拡散コード63,0x00000000,0xFFFFFFFF,,0x16677C1,0x16677C1,UP_SS_LSFR_INIT_D-63
,アップリンクデータ拡散コード64,0x00000000,0xFFFFFFFF,,0x16936B1,0x16936B1,UP_SS_LSFR_INIT_D-64
,アップリンクデータ拡散コード65,0x00000000,0xFFFFFFFF,,0x16BAE19,0x16BAE19,UP_SS_LSFR_INIT_D-65
,アップリンクデータ拡散コード66,0x00000000,0xFFFFFFFF,,0x16D0B40,0x16D0B40,UP_SS_LSFR_INIT_D-66
,アップリンクデータ拡散コード67,0x00000000,0xFFFFFFFF,,0x16EEC20,0x16EEC20,UP_SS_LSFR_INIT_D-67
,アップリンクデータ拡散コード68,0x00000000,0xFFFFFFFF,,0x1709525,0x1709525,UP_SS_LSFR_INIT_D-68
,アップリンクデータ拡散コード69,0x00000000,0xFFFFFFFF,,0x1719CDC,0x1719CDC,UP_SS_LSFR_INIT_D-69
,アップリンクデータ拡散コード70,0x00000000,0xFFFFFFFF,,0x171CCF2,0x171CCF2,UP_SS_LSFR_INIT_D-70
,アップリンクデータ拡散コード71,0x00000000,0xFFFFFFFF,,0x188F95E,0x188F95E,UP_SS_LSFR_INIT_D-71
,アップリンクデータ拡散コード72,0x00000000,0xFFFFFFFF,,0x189B95D,0x189B95D,UP_SS_LSFR_INIT_D-72
,アップリンクデータ拡散コード73,0x00000000,0xFFFFFFFF,,0x194873F,0x194873F,UP_SS_LSFR_INIT_D-73
,アップリンクデータ拡散コード74,0x00000000,0xFFFFFFFF,,0x199368E,0x199368E,UP_SS_LSFR_INIT_D-74
,アップリンクデータ拡散コード75,0x00000000,0xFFFFFFFF,,0x199D526,0x199D526,UP_SS_LSFR_INIT_D-75
,アップリンクデータ拡散コード76,0x00000000,0xFFFFFFFF,,0x19CC1C4,0x19CC1C4,UP_SS_LSFR_INIT_D-76
,アップリンクデータ拡散コード77,0x00000000,0xFFFFFFFF,,0x19EF52F,0x19EF52F,UP_SS_LSFR_INIT_D-77
,アップリンクデータ拡散コード78,0x00000000,0xFFFFFFFF,,0x1A2E3D3,0x1A2E3D3,UP_SS_LSFR_INIT_D-78
,アップリンクデータ拡散コード79,0x00000000,0xFFFFFFFF,,0x1A36B97,0x1A36B97,UP_SS_LSFR_INIT_D-79
,アップリンクデータ拡散コード80,0x00000000,0xFFFFFFFF,,0x1A39B38,0x1A39B38,UP_SS_LSFR_INIT_D-80
,アップリンクデータ拡散コード81,0x00000000,0xFFFFFFFF,,0x1A42F1D,0x1A42F1D,UP_SS_LSFR_INIT_D-81
,アップリンクデータ拡散コード82,0x00000000,0xFFFFFFFF,,0x1A45BEF,0x1A45BEF,UP_SS_LSFR_INIT_D-82
,アップリンクデータ拡散コード83,0x00000000,0xFFFFFFFF,,0x1A6634E,0x1A6634E,UP_SS_LSFR_INIT_D-83
,アップリンクデータ拡散コード84,0x00000000,0xFFFFFFFF,,0x1B6023B,0x1B6023B,UP_SS_LSFR_INIT_D-84
,アップリンクデータ拡散コード85,0x00000000,0xFFFFFFFF,,0x1B6301B,0x1B6301B,UP_SS_LSFR_INIT_D-85
,アップリンクデータ拡散コード86,0x00000000,0xFFFFFFFF,,0x1BC6E17,0x1BC6E17,UP_SS_LSFR_INIT_D-86
,アップリンクデータ拡散コード87,0x00000000,0xFFFFFFFF,,0x1BD8EA7,0x1BD8EA7,UP_SS_LSFR_INIT_D-87
,アップリンクデータ拡散コード88,0x00000000,0xFFFFFFFF,,0x1BE6CD3,0x1BE6CD3,UP_SS_LSFR_INIT_D-88
,アップリンクデータ拡散コード89,0x00000000,0xFFFFFFFF,,0x1CA9077,0x1CA9077,UP_SS_LSFR_INIT_D-89
,アップリンクデータ拡散コード90,0x00000000,0xFFFFFFFF,,0x1CF3CA6,0x1CF3CA6,UP_SS_LSFR_INIT_D-90
,アップリンクデータ拡散コード91,0x00000000,0xFFFFFFFF,,0x1DC088F,0x1DC088F,UP_SS_LSFR_INIT_D-91
,アップリンクデータ拡散コード92,0x00000000,0xFFFFFFFF,,0x1DCF325,0x1DCF325,UP_SS_LSFR_INIT_D-92
,アップリンクデータ拡散コード93,0x00000000,0xFFFFFFFF,,0x1E2C7C2,0x1E2C7C2,UP_SS_LSFR_INIT_D-93
,アップリンクデータ拡散コード94,0x00000000,0xFFFFFFFF,,0x1E9F2A1,0x1E9F2A1,UP_SS_LSFR_INIT_D-94
,アップリンクデータ拡散コード95,0x00000000,0xFFFFFFFF,,0x1EBB0BB,0x1EBB0BB,UP_SS_LSFR_INIT_D-95
,アップリンクデータ拡散コード96,0x00000000,0xFFFFFFFF,,0x1F4DEF8,0x1F4DEF8,UP_SS_LSFR_INIT_D-96
,アップリンクデータ拡散コード97,0x00000000,0xFFFFFFFF,,0x1FC9486,0x1FC9486,UP_SS_LSFR_INIT_D-97
,アップリンクデータ拡散コード98,0x00000000,0xFFFFFFFF,,0x1FF8A86,0x1FF8A86,UP_SS_LSFR_INIT_D-98
,アップリンクデータ拡散コード99,0x00000000,0xFFFFFFFF,,0x202EE37,0x202EE37,UP_SS_LSFR_INIT_D-99
,アップリンクデータ拡散コード100,0x00000000,0xFFFFFFFF,,0x2115A5F,0x2115A5F,UP_SS_LSFR_INIT_D-100
,アップリンクデータ拡散コード101,0x00000000,0xFFFFFFFF,,0x2157341,0x2157341,UP_SS_LSFR_INIT_D-101
,アップリンクデータ拡散コード102,0x00000000,0xFFFFFFFF,,0x22AB3C8,0x22AB3C8,UP_SS_LSFR_INIT_D-102
,アップリンクデータ拡散コード103,0x00000000,0xFFFFFFFF,,0x22BC6B4,0x22BC6B4,UP_SS_LSFR_INIT_D-103
,アップリンクデータ拡散コード104,0x00000000,0xFFFFFFFF,,0x234313E,0x234313E,UP_SS_LSFR_INIT_D-104
,アップリンクデータ拡散コード105,0x00000000,0xFFFFFFFF,,0x242D289,0x242D289,UP_SS_LSFR_INIT_D-105
,アップリンクデータ拡散コード106,0x00000000,0xFFFFFFFF,,0x2477805,0x2477805,UP_SS_LSFR_INIT_D-106
,アップリンクデータ拡散コード107,0x00000000,0xFFFFFFFF,,0x249B727,0x249B727,UP_SS_LSFR_INIT_D-107
,アップリンクデータ拡散コード108,0x00000000,0xFFFFFFFF,,0x24E7652,0x24E7652,UP_SS_LSFR_INIT_D-108
,アップリンクデータ拡散コード109,0x00000000,0xFFFFFFFF,,0x25008B4,0x25008B4,UP_SS_LSFR_INIT_D-109
,アップリンクデータ拡散コード110,0x00000000,0xFFFFFFFF,,0x252E467,0x252E467,UP_SS_LSFR_INIT_D-110
,アップリンクデータ拡散コード111,0x00000000,0xFFFFFFFF,,0x25ED184,0x25ED184,UP_SS_LSFR_INIT_D-111
,アップリンクデータ拡散コード112,0x00000000,0xFFFFFFFF,,0x25F663E,0x25F663E,UP_SS_LSFR_INIT_D-112
,アップリンクデータ拡散コード113,0x00000000,0xFFFFFFFF,,0x265F851,0x265F851,UP_SS_LSFR_INIT_D-113
,アップリンクデータ拡散コード114,0x00000000,0xFFFFFFFF,,0x269D44D,0x269D44D,UP_SS_LSFR_INIT_D-114
,アップリンクデータ拡散コード115,0x00000000,0xFFFFFFFF,,0x26D6383,0x26D6383,UP_SS_LSFR_INIT_D-115
,アップリンクデータ拡散コード116,0x00000000,0xFFFFFFFF,,0x278C848,0x278C848,UP_SS_LSFR_INIT_D-116
,アップリンクデータ拡散コード117,0x00000000,0xFFFFFFFF,,0x27F122F,0x27F122F,UP_SS_LSFR_INIT_D-117
,アップリンクデータ拡散コード118,0x00000000,0xFFFFFFFF,,0x2877D31,0x2877D31,UP_SS_LSFR_INIT_D-118
,アップリンクデータ拡散コード119,0x00000000,0xFFFFFFFF,,0x2890AE1,0x2890AE1,UP_SS_LSFR_INIT_D-119
,アップリンクデータ拡散コード120,0x00000000,0xFFFFFFFF,,0x292E1EF,0x292E1EF,UP_SS_LSFR_INIT_D-120
,アップリンクデータ拡散コード121,0x00000000,0xFFFFFFFF,,0x295EF8E,0x295EF8E,UP_SS_LSFR_INIT_D-121
,アップリンクデータ拡散コード122,0x00000000,0xFFFFFFFF,,0x2979005,0x2979005,UP_SS_LSFR_INIT_D-122
,アップリンクデータ拡散コード123,0x00000000,0xFFFFFFFF,,0x2979E94,0x2979E94,UP_SS_LSFR_INIT_D-123
,アップリンクデータ拡散コード124,0x00000000,0xFFFFFFFF,,0x29832E5,0x29832E5,UP_SS_LSFR_INIT_D-124
,アップリンクデータ拡散コード125,0x00000000,0xFFFFFFFF,,0x2A25C99,0x2A25C99,UP_SS_LSFR_INIT_D-125
,アップリンクデータ拡散コード126,0x00000000,0xFFFFFFFF,,0x2A28F6F,0x2A28F6F,UP_SS_LSFR_INIT_D-126
,アップリンクデータ拡散コード127,0x00000000,0xFFFFFFFF,,0x2A6776E,0x2A6776E,UP_SS_LSFR_INIT_D-127
,アップリンクデータ拡散コード128,0x00000000,0xFFFFFFFF,,0x2ABC047,0x2ABC047,UP_SS_LSFR_INIT_D-128
,アップリンクデータ拡散コード129,0x00000000,0xFFFFFFFF,,0x2B4E0D6,0x2B4E0D6,UP_SS_LSFR_INIT_D-129
,アップリンクデータ拡散コード130,0x00000000,0xFFFFFFFF,,0x2B513F4,0x2B513F4,UP_SS_LSFR_INIT_D-130
,アップリンクデータ拡散コード131,0x00000000,0xFFFFFFFF,,0x2C06019,0x2C06019,UP_SS_LSFR_INIT_D-131
,アップリンクデータ拡散コード132,0x00000000,0xFFFFFFFF,,0x2C2AF03,0x2C2AF03,UP_SS_LSFR_INIT_D-132
,アップリンクデータ拡散コード133,0x00000000,0xFFFFFFFF,,0x2C4508F,0x2C4508F,UP_SS_LSFR_INIT_D-133
,アップリンクデータ拡散コード134,0x00000000,0xFFFFFFFF,,0x2C9FA0C,0x2C9FA0C,UP_SS_LSFR_INIT_D-134
,アップリンクデータ拡散コード135,0x00000000,0xFFFFFFFF,,0x2CB0437,0x2CB0437,UP_SS_LSFR_INIT_D-135
,アップリンクデータ拡散コード136,0x00000000,0xFFFFFFFF,,0x2CE9D1E,0x2CE9D1E,UP_SS_LSFR_INIT_D-136
,アップリンクデータ拡散コード137,0x00000000,0xFFFFFFFF,,0x2CFA39C,0x2CFA39C,UP_SS_LSFR_INIT_D-137
,アップリンクデータ拡散コード138,0x00000000,0xFFFFFFFF,,0x2D3D94A,0x2D3D94A,UP_SS_LSFR_INIT_D-138
,アップリンクデータ拡散コード139,0x00000000,0xFFFFFFFF,,0x2DB0A82,0x2DB0A82,UP_SS_LSFR_INIT_D-139
,アップリンクデータ拡散コード140,0x00000000,0xFFFFFFFF,,0x2DEB49F,0x2DEB49F,UP_SS_LSFR_INIT_D-140
,アップリンクデータ拡散コード141,0x00000000,0xFFFFFFFF,,0x2F16151,0x2F16151,UP_SS_LSFR_INIT_D-141
,アップリンクデータ拡散コード142,0x00000000,0xFFFFFFFF,,0x2F73811,0x2F73811,UP_SS_LSFR_INIT_D-142
,アップリンクデータ拡散コード143,0x00000000,0xFFFFFFFF,,0x2F9A5D4,0x2F9A5D4,UP_SS_LSFR_INIT_D-143
,アップリンクデータ拡散コード144,0x00000000,0xFFFFFFFF,,0x300A57A,0x300A57A,UP_SS_LSFR_INIT_D-144
,アップリンクデータ拡散コード145,0x00000000,0xFFFFFFFF,,0x300F838,0x300F838,UP_SS_LSFR_INIT_D-145
,アップリンクデータ拡散コード146,0x00000000,0xFFFFFFFF,,0x301718C,0x301718C,UP_SS_LSFR_INIT_D-146
,アップリンクデータ拡散コード147,0x00000000,0xFFFFFFFF,,0x303AAE4,0x303AAE4,UP_SS_LSFR_INIT_D-147
,アップリンクデータ拡散コード148,0x00000000,0xFFFFFFFF,,0x30BFDC2,0x30BFDC2,UP_SS_LSFR_INIT_D-148
,アップリンクデータ拡散コード149,0x00000000,0xFFFFFFFF,,0x310EEB6,0x310EEB6,UP_SS_LSFR_INIT_D-149
,アップリンクデータ拡散コード150,0x00000000,0xFFFFFFFF,,0x312BE5F,0x312BE5F,UP_SS_LSFR_INIT_D-150
,アップリンクデータ拡散コード151,0x00000000,0xFFFFFFFF,,0x31750CA,0x31750CA,UP_SS_LSFR_INIT_D-151
,アップリンクデータ拡散コード152,0x00000000,0xFFFFFFFF,,0x336DC69,0x336DC69,UP_SS_LSFR_INIT_D-152
,アップリンクデータ拡散コード153,0x00000000,0xFFFFFFFF,,0x337787C,0x337787C,UP_SS_LSFR_INIT_D-153
,アップリンクデータ拡散コード154,0x00000000,0xFFFFFFFF,,0x33E1B8C,0x33E1B8C,UP_SS_LSFR_INIT_D-154
,アップリンクデータ拡散コード155,0x00000000,0xFFFFFFFF,,0x33ED4DC,0x33ED4DC,UP_SS_LSFR_INIT_D-155
,アップリンクデータ拡散コード156,0x00000000,0xFFFFFFFF,,0x3432245,0x3432245,UP_SS_LSFR_INIT_D-156
,アップリンクデータ拡散コード157,0x00000000,0xFFFFFFFF,,0x34CE1CD,0x34CE1CD,UP_SS_LSFR_INIT_D-157
,アップリンクデータ拡散コード158,0x00000000,0xFFFFFFFF,,0x35197FE,0x35197FE,UP_SS_LSFR_INIT_D-158
,アップリンクデータ拡散コード159,0x00000000,0xFFFFFFFF,,0x352CAA0,0x352CAA0,UP_SS_LSFR_INIT_D-159
,アップリンクデータ拡散コード160,0x00000000,0xFFFFFFFF,,0x354FFA9,0x354FFA9,UP_SS_LSFR_INIT_D-160
,アップリンクデータ拡散コード161,0x00000000,0xFFFFFFFF,,0x3573CCD,0x3573CCD,UP_SS_LSFR_INIT_D-161
,アップリンクデータ拡散コード162,0x00000000,0xFFFFFFFF,,0x357F151,0x357F151,UP_SS_LSFR_INIT_D-162
,アップリンクデータ拡散コード163,0x00000000,0xFFFFFFFF,,0x359BEEC,0x359BEEC,UP_SS_LSFR_INIT_D-163
,アップリンクデータ拡散コード164,0x00000000,0xFFFFFFFF,,0x35DEDC6,0x35DEDC6,UP_SS_LSFR_INIT_D-164
,アップリンクデータ拡散コード165,0x00000000,0xFFFFFFFF,,0x361B788,0x361B788,UP_SS_LSFR_INIT_D-165
,アップリンクデータ拡散コード166,0x00000000,0xFFFFFFFF,,0x361DC09,0x361DC09,UP_SS_LSFR_INIT_D-166
,アップリンクデータ拡散コード167,0x00000000,0xFFFFFFFF,,0x364E56D,0x364E56D,UP_SS_LSFR_INIT_D-167
,アップリンクデータ拡散コード168,0x00000000,0xFFFFFFFF,,0x36F0EC7,0x36F0EC7,UP_SS_LSFR_INIT_D-168
,アップリンクデータ拡散コード169,0x00000000,0xFFFFFFFF,,0x36F5B7E,0x36F5B7E,UP_SS_LSFR_INIT_D-169
,アップリンクデータ拡散コード170,0x00000000,0xFFFFFFFF,,0x3773D40,0x3773D40,UP_SS_LSFR_INIT_D-170
,アップリンクデータ拡散コード171,0x00000000,0xFFFFFFFF,,0x37B148E,0x37B148E,UP_SS_LSFR_INIT_D-171
,アップリンクデータ拡散コード172,0x00000000,0xFFFFFFFF,,0x37DA758,0x37DA758,UP_SS_LSFR_INIT_D-172
,アップリンクデータ拡散コード173,0x00000000,0xFFFFFFFF,,0x38FEA01,0x38FEA01,UP_SS_LSFR_INIT_D-173
,アップリンクデータ拡散コード174,0x00000000,0xFFFFFFFF,,0x3940F39,0x3940F39,UP_SS_LSFR_INIT_D-174
,アップリンクデータ拡散コード175,0x00000000,0xFFFFFFFF,,0x398C2BB,0x398C2BB,UP_SS_LSFR_INIT_D-175
,アップリンクデータ拡散コード176,0x00000000,0xFFFFFFFF,,0x39D5942,0x39D5942,UP_SS_LSFR_INIT_D-176
,アップリンクデータ拡散コード177,0x00000000,0xFFFFFFFF,,0x3B12A25,0x3B12A25,UP_SS_LSFR_INIT_D-177
,アップリンクデータ拡散コード178,0x00000000,0xFFFFFFFF,,0x3B1F80A,0x3B1F80A,UP_SS_LSFR_INIT_D-178
,アップリンクデータ拡散コード179,0x00000000,0xFFFFFFFF,,0x3BA9E33,0x3BA9E33,UP_SS_LSFR_INIT_D-179
,アップリンクデータ拡散コード180,0x00000000,0xFFFFFFFF,,0x3BE4F75,0x3BE4F75,UP_SS_LSFR_INIT_D-180
,アップリンクデータ拡散コード181,0x00000000,0xFFFFFFFF,,0x3C325FE,0x3C325FE,UP_SS_LSFR_INIT_D-181
,アップリンクデータ拡散コード182,0x00000000,0xFFFFFFFF,,0x3C78EF6,0x3C78EF6,UP_SS_LSFR_INIT_D-182
,アップリンクデータ拡散コード183,0x00000000,0xFFFFFFFF,,0x3C84661,0x3C84661,UP_SS_LSFR_INIT_D-183
,アップリンクデータ拡散コード184,0x00000000,0xFFFFFFFF,,0x3CC74B7,0x3CC74B7,UP_SS_LSFR_INIT_D-184
,アップリンクデータ拡散コード185,0x00000000,0xFFFFFFFF,,0x3D369C1,0x3D369C1,UP_SS_LSFR_INIT_D-185
,アップリンクデータ拡散コード186,0x00000000,0xFFFFFFFF,,0x3D442E5,0x3D442E5,UP_SS_LSFR_INIT_D-186
,アップリンクデータ拡散コード187,0x00000000,0xFFFFFFFF,,0x3D9263C,0x3D9263C,UP_SS_LSFR_INIT_D-187
,アップリンクデータ拡散コード188,0x00000000,0xFFFFFFFF,,0x3DAD9A8,0x3DAD9A8,UP_SS_LSFR_INIT_D-188
,アップリンクデータ拡散コード189,0x00000000,0xFFFFFFFF,,0x3DB8089,0x3DB8089,UP_SS_LSFR_INIT_D-189
,アップリンクデータ拡散コード190,0x00000000,0xFFFFFFFF,,0x3EAF2D8,0x3EAF2D8,UP_SS_LSFR_INIT_D-190
,アップリンクデータ拡散コード191,0x00000000,0xFFFFFFFF,,0x3F02991,0x3F02991,UP_SS_LSFR_INIT_D-191
,アップリンクデータ拡散コード192,0x00000000,0xFFFFFFFF,,0x3F1FA62,0x3F1FA62,UP_SS_LSFR_INIT_D-192
,アップリンクデータ拡散コード193,0x00000000,0xFFFFFFFF,,0x3F2DCBB,0x3F2DCBB,UP_SS_LSFR_INIT_D-193
,アップリンクデータ拡散コード194,0x00000000,0xFFFFFFFF,,0x3F39AD7,0x3F39AD7,UP_SS_LSFR_INIT_D-194
,アップリンクデータ拡散コード195,0x00000000,0xFFFFFFFF,,0x3F96C7A,0x3F96C7A,UP_SS_LSFR_INIT_D-195
,アップリンクデータ拡散コード196,0x00000000,0xFFFFFFFF,,0x403DCC3,0x403DCC3,UP_SS_LSFR_INIT_D-196
,アップリンクデータ拡散コード197,0x00000000,0xFFFFFFFF,,0x4068512,0x4068512,UP_SS_LSFR_INIT_D-197
,アップリンクデータ拡散コード198,0x00000000,0xFFFFFFFF,,0x408CAC3,0x408CAC3,UP_SS_LSFR_INIT_D-198
,アップリンクデータ拡散コード199,0x00000000,0xFFFFFFFF,,0x40C7626,0x40C7626,UP_SS_LSFR_INIT_D-199
,アップリンクデータ拡散コード200,0x00000000,0xFFFFFFFF,,0x41148F8,0x41148F8,UP_SS_LSFR_INIT_D-200
,アップリンクデータ拡散コード201,0x00000000,0xFFFFFFFF,,0x416509E,0x416509E,UP_SS_LSFR_INIT_D-201
,アップリンクデータ拡散コード202,0x00000000,0xFFFFFFFF,,0x41660A2,0x41660A2,UP_SS_LSFR_INIT_D-202
,アップリンクデータ拡散コード203,0x00000000,0xFFFFFFFF,,0x41B5CC0,0x41B5CC0,UP_SS_LSFR_INIT_D-203
,アップリンクデータ拡散コード204,0x00000000,0xFFFFFFFF,,0x41DF8C8,0x41DF8C8,UP_SS_LSFR_INIT_D-204
,アップリンクデータ拡散コード205,0x00000000,0xFFFFFFFF,,0x424D2E6,0x424D2E6,UP_SS_LSFR_INIT_D-205
,アップリンクデータ拡散コード206,0x00000000,0xFFFFFFFF,,0x425D73A,0x425D73A,UP_SS_LSFR_INIT_D-206
,アップリンクデータ拡散コード207,0x00000000,0xFFFFFFFF,,0x425FE68,0x425FE68,UP_SS_LSFR_INIT_D-207
,アップリンクデータ拡散コード208,0x00000000,0xFFFFFFFF,,0x428A0D9,0x428A0D9,UP_SS_LSFR_INIT_D-208
,アップリンクデータ拡散コード209,0x00000000,0xFFFFFFFF,,0x42CB038,0x42CB038,UP_SS_LSFR_INIT_D-209
,アップリンクデータ拡散コード210,0x00000000,0xFFFFFFFF,,0x43162FD,0x43162FD,UP_SS_LSFR_INIT_D-210
,アップリンクデータ拡散コード211,0x00000000,0xFFFFFFFF,,0x435F721,0x435F721,UP_SS_LSFR_INIT_D-211
,アップリンクデータ拡散コード212,0x00000000,0xFFFFFFFF,,0x4370711,0x4370711,UP_SS_LSFR_INIT_D-212
,アップリンクデータ拡散コード213,0x00000000,0xFFFFFFFF,,0x4385F76,0x4385F76,UP_SS_LSFR_INIT_D-213
,アップリンクデータ拡散コード214,0x00000000,0xFFFFFFFF,,0x43A7AD8,0x43A7AD8,UP_SS_LSFR_INIT_D-214
,アップリンクデータ拡散コード215,0x00000000,0xFFFFFFFF,,0x43BEF7E,0x43BEF7E,UP_SS_LSFR_INIT_D-215
,アップリンクデータ拡散コード216,0x00000000,0xFFFFFFFF,,0x43DAACC,0x43DAACC,UP_SS_LSFR_INIT_D-216
,アップリンクデータ拡散コード217,0x00000000,0xFFFFFFFF,,0x443EB14,0x443EB14,UP_SS_LSFR_INIT_D-217
,アップリンクデータ拡散コード218,0x00000000,0xFFFFFFFF,,0x446F459,0x446F459,UP_SS_LSFR_INIT_D-218
,アップリンクデータ拡散コード219,0x00000000,0xFFFFFFFF,,0x4487D41,0x4487D41,UP_SS_LSFR_INIT_D-219
,アップリンクデータ拡散コード220,0x00000000,0xFFFFFFFF,,0x44921FD,0x44921FD,UP_SS_LSFR_INIT_D-220
,アップリンクデータ拡散コード221,0x00000000,0xFFFFFFFF,,0x44EFBFC,0x44EFBFC,UP_SS_LSFR_INIT_D-221
,アップリンクデータ拡散コード222,0x00000000,0xFFFFFFFF,,0x467C467,0x467C467,UP_SS_LSFR_INIT_D-222
,アップリンクデータ拡散コード223,0x00000000,0xFFFFFFFF,,0x46F3A65,0x46F3A65,UP_SS_LSFR_INIT_D-223
,アップリンクデータ拡散コード224,0x00000000,0xFFFFFFFF,,0x473F0CF,0x473F0CF,UP_SS_LSFR_INIT_D-224
,アップリンクデータ拡散コード225,0x00000000,0xFFFFFFFF,,0x4797F3E,0x4797F3E,UP_SS_LSFR_INIT_D-225
,アップリンクデータ拡散コード226,0x00000000,0xFFFFFFFF,,0x47D0CAD,0x47D0CAD,UP_SS_LSFR_INIT_D-226
,アップリンクデータ拡散コード227,0x00000000,0xFFFFFFFF,,0x47D1904,0x47D1904,UP_SS_LSFR_INIT_D-227
,アップリンクデータ拡散コード228,0x00000000,0xFFFFFFFF,,0x47D556D,0x47D556D,UP_SS_LSFR_INIT_D-228
,アップリンクデータ拡散コード229,0x00000000,0xFFFFFFFF,,0x47EFFC2,0x47EFFC2,UP_SS_LSFR_INIT_D-229
,アップリンクデータ拡散コード230,0x00000000,0xFFFFFFFF,,0x4821A9A,0x4821A9A,UP_SS_LSFR_INIT_D-230
,アップリンクデータ拡散コード231,0x00000000,0xFFFFFFFF,,0x48667AB,0x48667AB,UP_SS_LSFR_INIT_D-231
,アップリンクデータ拡散コード232,0x00000000,0xFFFFFFFF,,0x489CE7E,0x489CE7E,UP_SS_LSFR_INIT_D-232
,アップリンクデータ拡散コード233,0x00000000,0xFFFFFFFF,,0x48EDD40,0x48EDD40,UP_SS_LSFR_INIT_D-233
,アップリンクデータ拡散コード234,0x00000000,0xFFFFFFFF,,0x4917672,0x4917672,UP_SS_LSFR_INIT_D-234
,アップリンクデータ拡散コード235,0x00000000,0xFFFFFFFF,,0x499CC03,0x499CC03,UP_SS_LSFR_INIT_D-235
,アップリンクデータ拡散コード236,0x00000000,0xFFFFFFFF,,0x49E28DB,0x49E28DB,UP_SS_LSFR_INIT_D-236
,アップリンクデータ拡散コード237,0x00000000,0xFFFFFFFF,,0x4A310D4,0x4A310D4,UP_SS_LSFR_INIT_D-237
,アップリンクデータ拡散コード238,0x00000000,0xFFFFFFFF,,0x4A76CD3,0x4A76CD3,UP_SS_LSFR_INIT_D-238
,アップリンクデータ拡散コード239,0x00000000,0xFFFFFFFF,,0x4A7B13A,0x4A7B13A,UP_SS_LSFR_INIT_D-239
,アップリンクデータ拡散コード240,0x00000000,0xFFFFFFFF,,0x4A87675,0x4A87675,UP_SS_LSFR_INIT_D-240
,アップリンクデータ拡散コード241,0x00000000,0xFFFFFFFF,,0x4A9E5E0,0x4A9E5E0,UP_SS_LSFR_INIT_D-241
,アップリンクデータ拡散コード242,0x00000000,0xFFFFFFFF,,0x4A9EEC9,0x4A9EEC9,UP_SS_LSFR_INIT_D-242
,アップリンクデータ拡散コード243,0x00000000,0xFFFFFFFF,,0x4AC813B,0x4AC813B,UP_SS_LSFR_INIT_D-243
,アップリンクデータ拡散コード244,0x00000000,0xFFFFFFFF,,0x4AD4BBF,0x4AD4BBF,UP_SS_LSFR_INIT_D-244
,アップリンクデータ拡散コード245,0x00000000,0xFFFFFFFF,,0x4B72FE2,0x4B72FE2,UP_SS_LSFR_INIT_D-245
,アップリンクデータ拡散コード246,0x00000000,0xFFFFFFFF,,0x4C33409,0x4C33409,UP_SS_LSFR_INIT_D-246
,アップリンクデータ拡散コード247,0x00000000,0xFFFFFFFF,,0x4CAC62E,0x4CAC62E,UP_SS_LSFR_INIT_D-247
,アップリンクデータ拡散コード248,0x00000000,0xFFFFFFFF,,0x4E87717,0x4E87717,UP_SS_LSFR_INIT_D-248
,アップリンクデータ拡散コード249,0x00000000,0xFFFFFFFF,,0x4EA82D7,0x4EA82D7,UP_SS_LSFR_INIT_D-249
,アップリンクデータ拡散コード250,0x00000000,0xFFFFFFFF,,0x4FAE626,0x4FAE626,UP_SS_LSFR_INIT_D-250
,アップリンクデータ拡散コード251,0x00000000,0xFFFFFFFF,,0x4FDE85C,0x4FDE85C,UP_SS_LSFR_INIT_D-251
,アップリンクデータ拡散コード252,0x00000000,0xFFFFFFFF,,0x4FFE99A,0x4FFE99A,UP_SS_LSFR_INIT_D-252
,アップリンクデータ拡散コード253,0x00000000,0xFFFFFFFF,,0x507BC0B,0x507BC0B,UP_SS_LSFR_INIT_D-253
,アップリンクデータ拡散コード254,0x00000000,0xFFFFFFFF,,0x50DD15C,0x50DD15C,UP_SS_LSFR_INIT_D-254
,アップリンクデータ拡散コード255,0x00000000,0xFFFFFFFF,,0x51D630E,0x51D630E,UP_SS_LSFR_INIT_D-255
,アップリンクデータ拡散コード256,0x00000000,0xFFFFFFFF,,0x5302DBF,0x5302DBF,UP_SS_LSFR_INIT_D-256
,アップリンクデータ拡散コード257,0x00000000,0xFFFFFFFF,,0x530DE96,0x530DE96,UP_SS_LSFR_INIT_D-257
,アップリンクデータ拡散コード258,0x00000000,0xFFFFFFFF,,0x538AA8B,0x538AA8B,UP_SS_LSFR_INIT_D-258
,アップリンクデータ拡散コード259,0x00000000,0xFFFFFFFF,,0x53C5B2F,0x53C5B2F,UP_SS_LSFR_INIT_D-259
,アップリンクデータ拡散コード260,0x00000000,0xFFFFFFFF,,0x546502A,0x546502A,UP_SS_LSFR_INIT_D-260
,アップリンクデータ拡散コード261,0x00000000,0xFFFFFFFF,,0x549E5FE,0x549E5FE,UP_SS_LSFR_INIT_D-261
,アップリンクデータ拡散コード262,0x00000000,0xFFFFFFFF,,0x54C9563,0x54C9563,UP_SS_LSFR_INIT_D-262
,アップリンクデータ拡散コード263,0x00000000,0xFFFFFFFF,,0x552E372,0x552E372,UP_SS_LSFR_INIT_D-263
,アップリンクデータ拡散コード264,0x00000000,0xFFFFFFFF,,0x5540D4A,0x5540D4A,UP_SS_LSFR_INIT_D-264
,アップリンクデータ拡散コード265,0x00000000,0xFFFFFFFF,,0x5544EF8,0x5544EF8,UP_SS_LSFR_INIT_D-265
,アップリンクデータ拡散コード266,0x00000000,0xFFFFFFFF,,0x5545805,0x5545805,UP_SS_LSFR_INIT_D-266
,アップリンクデータ拡散コード267,0x00000000,0xFFFFFFFF,,0x5594ABE,0x5594ABE,UP_SS_LSFR_INIT_D-267
,アップリンクデータ拡散コード268,0x00000000,0xFFFFFFFF,,0x55A6060,0x55A6060,UP_SS_LSFR_INIT_D-268
,アップリンクデータ拡散コード269,0x00000000,0xFFFFFFFF,,0x55ECD33,0x55ECD33,UP_SS_LSFR_INIT_D-269
,アップリンクデータ拡散コード270,0x00000000,0xFFFFFFFF,,0x5641919,0x5641919,UP_SS_LSFR_INIT_D-270
,アップリンクデータ拡散コード271,0x00000000,0xFFFFFFFF,,0x56A5B14,0x56A5B14,UP_SS_LSFR_INIT_D-271
,アップリンクデータ拡散コード272,0x00000000,0xFFFFFFFF,,0x56DCFDF,0x56DCFDF,UP_SS_LSFR_INIT_D-272
,アップリンクデータ拡散コード273,0x00000000,0xFFFFFFFF,,0x56E0E5C,0x56E0E5C,UP_SS_LSFR_INIT_D-273
,アップリンクデータ拡散コード274,0x00000000,0xFFFFFFFF,,0x5713FB1,0x5713FB1,UP_SS_LSFR_INIT_D-274
,アップリンクデータ拡散コード275,0x00000000,0xFFFFFFFF,,0x572F9E1,0x572F9E1,UP_SS_LSFR_INIT_D-275
,アップリンクデータ拡散コード276,0x00000000,0xFFFFFFFF,,0x575CDF6,0x575CDF6,UP_SS_LSFR_INIT_D-276
,アップリンクデータ拡散コード277,0x00000000,0xFFFFFFFF,,0x5763B12,0x5763B12,UP_SS_LSFR_INIT_D-277
,アップリンクデータ拡散コード278,0x00000000,0xFFFFFFFF,,0x584E56F,0x584E56F,UP_SS_LSFR_INIT_D-278
,アップリンクデータ拡散コード279,0x00000000,0xFFFFFFFF,,0x599790A,0x599790A,UP_SS_LSFR_INIT_D-279
,アップリンクデータ拡散コード280,0x00000000,0xFFFFFFFF,,0x599E7A6,0x599E7A6,UP_SS_LSFR_INIT_D-280
,アップリンクデータ拡散コード281,0x00000000,0xFFFFFFFF,,0x5A5907C,0x5A5907C,UP_SS_LSFR_INIT_D-281
,アップリンクデータ拡散コード282,0x00000000,0xFFFFFFFF,,0x5A66D9A,0x5A66D9A,UP_SS_LSFR_INIT_D-282
,アップリンクデータ拡散コード283,0x00000000,0xFFFFFFFF,,0x5B13765,0x5B13765,UP_SS_LSFR_INIT_D-283
,アップリンクデータ拡散コード284,0x00000000,0xFFFFFFFF,,0x5B2F8FF,0x5B2F8FF,UP_SS_LSFR_INIT_D-284
,アップリンクデータ拡散コード285,0x00000000,0xFFFFFFFF,,0x5B78074,0x5B78074,UP_SS_LSFR_INIT_D-285
,アップリンクデータ拡散コード286,0x00000000,0xFFFFFFFF,,0x5BB73D7,0x5BB73D7,UP_SS_LSFR_INIT_D-286
,アップリンクデータ拡散コード287,0x00000000,0xFFFFFFFF,,0x5BB87D5,0x5BB87D5,UP_SS_LSFR_INIT_D-287
,アップリンクデータ拡散コード288,0x00000000,0xFFFFFFFF,,0x5C26590,0x5C26590,UP_SS_LSFR_INIT_D-288
,アップリンクデータ拡散コード289,0x00000000,0xFFFFFFFF,,0x5C300BE,0x5C300BE,UP_SS_LSFR_INIT_D-289
,アップリンクデータ拡散コード290,0x00000000,0xFFFFFFFF,,0x5CB253D,0x5CB253D,UP_SS_LSFR_INIT_D-290
,アップリンクデータ拡散コード291,0x00000000,0xFFFFFFFF,,0x5CDACDF,0x5CDACDF,UP_SS_LSFR_INIT_D-291
,アップリンクデータ拡散コード292,0x00000000,0xFFFFFFFF,,0x5D0B51C,0x5D0B51C,UP_SS_LSFR_INIT_D-292
,アップリンクデータ拡散コード293,0x00000000,0xFFFFFFFF,,0x5D2E6AA,0x5D2E6AA,UP_SS_LSFR_INIT_D-293
,アップリンクデータ拡散コード294,0x00000000,0xFFFFFFFF,,0x5DD7312,0x5DD7312,UP_SS_LSFR_INIT_D-294
,アップリンクデータ拡散コード295,0x00000000,0xFFFFFFFF,,0x5DFE2B5,0x5DFE2B5,UP_SS_LSFR_INIT_D-295
,アップリンクデータ拡散コード296,0x00000000,0xFFFFFFFF,,0x5E1FA3A,0x5E1FA3A,UP_SS_LSFR_INIT_D-296
,アップリンクデータ拡散コード297,0x00000000,0xFFFFFFFF,,0x5E333C5,0x5E333C5,UP_SS_LSFR_INIT_D-297
,アップリンクデータ拡散コード298,0x00000000,0xFFFFFFFF,,0x5E46875,0x5E46875,UP_SS_LSFR_INIT_D-298
,アップリンクデータ拡散コード299,0x00000000,0xFFFFFFFF,,0x5EAC8DB,0x5EAC8DB,UP_SS_LSFR_INIT_D-299
,アップリンクデータ拡散コード300,0x00000000,0xFFFFFFFF,,0x5EF06CE,0x5EF06CE,UP_SS_LSFR_INIT_D-300
,アップリンクデータ拡散コード301,0x00000000,0xFFFFFFFF,,0x5F1A16C,0x5F1A16C,UP_SS_LSFR_INIT_D-301
,アップリンクデータ拡散コード302,0x00000000,0xFFFFFFFF,,0x6031AD3,0x6031AD3,UP_SS_LSFR_INIT_D-302
,アップリンクデータ拡散コード303,0x00000000,0xFFFFFFFF,,0x605329C,0x605329C,UP_SS_LSFR_INIT_D-303
,アップリンクデータ拡散コード304,0x00000000,0xFFFFFFFF,,0x605A0D6,0x605A0D6,UP_SS_LSFR_INIT_D-304
,アップリンクデータ拡散コード305,0x00000000,0xFFFFFFFF,,0x605BAB1,0x605BAB1,UP_SS_LSFR_INIT_D-305
,アップリンクデータ拡散コード306,0x00000000,0xFFFFFFFF,,0x61AC251,0x61AC251,UP_SS_LSFR_INIT_D-306
,アップリンクデータ拡散コード307,0x00000000,0xFFFFFFFF,,0x61ADE71,0x61ADE71,UP_SS_LSFR_INIT_D-307
,アップリンクデータ拡散コード308,0x00000000,0xFFFFFFFF,,0x61B4276,0x61B4276,UP_SS_LSFR_INIT_D-308
,アップリンクデータ拡散コード309,0x00000000,0xFFFFFFFF,,0x61EC8F5,0x61EC8F5,UP_SS_LSFR_INIT_D-309
,アップリンクデータ拡散コード310,0x00000000,0xFFFFFFFF,,0x62B4A5B,0x62B4A5B,UP_SS_LSFR_INIT_D-310
,アップリンクデータ拡散コード311,0x00000000,0xFFFFFFFF,,0x62C2622,0x62C2622,UP_SS_LSFR_INIT_D-311
,アップリンクデータ拡散コード312,0x00000000,0xFFFFFFFF,,0x62C4E88,0x62C4E88,UP_SS_LSFR_INIT_D-312
,アップリンクデータ拡散コード313,0x00000000,0xFFFFFFFF,,0x62CA62E,0x62CA62E,UP_SS_LSFR_INIT_D-313
,アップリンクデータ拡散コード314,0x00000000,0xFFFFFFFF,,0x6379EF9,0x6379EF9,UP_SS_LSFR_INIT_D-314
,アップリンクデータ拡散コード315,0x00000000,0xFFFFFFFF,,0x6450454,0x6450454,UP_SS_LSFR_INIT_D-315
,アップリンクデータ拡散コード316,0x00000000,0xFFFFFFFF,,0x6453F29,0x6453F29,UP_SS_LSFR_INIT_D-316
,アップリンクデータ拡散コード317,0x00000000,0xFFFFFFFF,,0x649AD02,0x649AD02,UP_SS_LSFR_INIT_D-317
,アップリンクデータ拡散コード318,0x00000000,0xFFFFFFFF,,0x6562124,0x6562124,UP_SS_LSFR_INIT_D-318
,アップリンクデータ拡散コード319,0x00000000,0xFFFFFFFF,,0x67A6A1E,0x67A6A1E,UP_SS_LSFR_INIT_D-319
,アップリンクデータ拡散コード320,0x00000000,0xFFFFFFFF,,0x6816A2B,0x6816A2B,UP_SS_LSFR_INIT_D-320
,アップリンクデータ拡散コード321,0x00000000,0xFFFFFFFF,,0x6886185,0x6886185,UP_SS_LSFR_INIT_D-321
,アップリンクデータ拡散コード322,0x00000000,0xFFFFFFFF,,0x689CC6B,0x689CC6B,UP_SS_LSFR_INIT_D-322
,アップリンクデータ拡散コード323,0x00000000,0xFFFFFFFF,,0x68E2623,0x68E2623,UP_SS_LSFR_INIT_D-323
,アップリンクデータ拡散コード324,0x00000000,0xFFFFFFFF,,0x68E94BE,0x68E94BE,UP_SS_LSFR_INIT_D-324
,アップリンクデータ拡散コード325,0x00000000,0xFFFFFFFF,,0x690D6D4,0x690D6D4,UP_SS_LSFR_INIT_D-325
,アップリンクデータ拡散コード326,0x00000000,0xFFFFFFFF,,0x69154FF,0x69154FF,UP_SS_LSFR_INIT_D-326
,アップリンクデータ拡散コード327,0x00000000,0xFFFFFFFF,,0x698FADD,0x698FADD,UP_SS_LSFR_INIT_D-327
,アップリンクデータ拡散コード328,0x00000000,0xFFFFFFFF,,0x69A8FC2,0x69A8FC2,UP_SS_LSFR_INIT_D-328
,アップリンクデータ拡散コード329,0x00000000,0xFFFFFFFF,,0x69B6691,0x69B6691,UP_SS_LSFR_INIT_D-329
,アップリンクデータ拡散コード330,0x00000000,0xFFFFFFFF,,0x69BBD13,0x69BBD13,UP_SS_LSFR_INIT_D-330
,アップリンクデータ拡散コード331,0x00000000,0xFFFFFFFF,,0x69C30D1,0x69C30D1,UP_SS_LSFR_INIT_D-331
,アップリンクデータ拡散コード332,0x00000000,0xFFFFFFFF,,0x69D2272,0x69D2272,UP_SS_LSFR_INIT_D-332
,アップリンクデータ拡散コード333,0x00000000,0xFFFFFFFF,,0x69E3E08,0x69E3E08,UP_SS_LSFR_INIT_D-333
,アップリンクデータ拡散コード334,0x00000000,0xFFFFFFFF,,0x6A00439,0x6A00439,UP_SS_LSFR_INIT_D-334
,アップリンクデータ拡散コード335,0x00000000,0xFFFFFFFF,,0x6AB1624,0x6AB1624,UP_SS_LSFR_INIT_D-335
,アップリンクデータ拡散コード336,0x00000000,0xFFFFFFFF,,0x6B3FC79,0x6B3FC79,UP_SS_LSFR_INIT_D-336
,アップリンクデータ拡散コード337,0x00000000,0xFFFFFFFF,,0x6B529F1,0x6B529F1,UP_SS_LSFR_INIT_D-337
,アップリンクデータ拡散コード338,0x00000000,0xFFFFFFFF,,0x6BAF389,0x6BAF389,UP_SS_LSFR_INIT_D-338
,アップリンクデータ拡散コード339,0x00000000,0xFFFFFFFF,,0x6C0A553,0x6C0A553,UP_SS_LSFR_INIT_D-339
,アップリンクデータ拡散コード340,0x00000000,0xFFFFFFFF,,0x6C1BB9B,0x6C1BB9B,UP_SS_LSFR_INIT_D-340
,アップリンクデータ拡散コード341,0x00000000,0xFFFFFFFF,,0x6C28D20,0x6C28D20,UP_SS_LSFR_INIT_D-341
,アップリンクデータ拡散コード342,0x00000000,0xFFFFFFFF,,0x6C858A9,0x6C858A9,UP_SS_LSFR_INIT_D-342
,アップリンクデータ拡散コード343,0x00000000,0xFFFFFFFF,,0x6D76F30,0x6D76F30,UP_SS_LSFR_INIT_D-343
,アップリンクデータ拡散コード344,0x00000000,0xFFFFFFFF,,0x6E06B36,0x6E06B36,UP_SS_LSFR_INIT_D-344
,アップリンクデータ拡散コード345,0x00000000,0xFFFFFFFF,,0x6E615B4,0x6E615B4,UP_SS_LSFR_INIT_D-345
,アップリンクデータ拡散コード346,0x00000000,0xFFFFFFFF,,0x6E6F333,0x6E6F333,UP_SS_LSFR_INIT_D-346
,アップリンクデータ拡散コード347,0x00000000,0xFFFFFFFF,,0x6E89269,0x6E89269,UP_SS_LSFR_INIT_D-347
,アップリンクデータ拡散コード348,0x00000000,0xFFFFFFFF,,0x6E9CFCD,0x6E9CFCD,UP_SS_LSFR_INIT_D-348
,アップリンクデータ拡散コード349,0x00000000,0xFFFFFFFF,,0x6E9DD5B,0x6E9DD5B,UP_SS_LSFR_INIT_D-349
,アップリンクデータ拡散コード350,0x00000000,0xFFFFFFFF,,0x6F17E61,0x6F17E61,UP_SS_LSFR_INIT_D-350
,アップリンクデータ拡散コード351,0x00000000,0xFFFFFFFF,,0x6F53C20,0x6F53C20,UP_SS_LSFR_INIT_D-351
,アップリンクデータ拡散コード352,0x00000000,0xFFFFFFFF,,0x6F75753,0x6F75753,UP_SS_LSFR_INIT_D-352
,アップリンクデータ拡散コード353,0x00000000,0xFFFFFFFF,,0x6F9AFB3,0x6F9AFB3,UP_SS_LSFR_INIT_D-353
,アップリンクデータ拡散コード354,0x00000000,0xFFFFFFFF,,0x70238DB,0x70238DB,UP_SS_LSFR_INIT_D-354
,アップリンクデータ拡散コード355,0x00000000,0xFFFFFFFF,,0x7078055,0x7078055,UP_SS_LSFR_INIT_D-355
,アップリンクデータ拡散コード356,0x00000000,0xFFFFFFFF,,0x70AD37A,0x70AD37A,UP_SS_LSFR_INIT_D-356
,アップリンクデータ拡散コード357,0x00000000,0xFFFFFFFF,,0x7118EDA,0x7118EDA,UP_SS_LSFR_INIT_D-357
,アップリンクデータ拡散コード358,0x00000000,0xFFFFFFFF,,0x71D1E9B,0x71D1E9B,UP_SS_LSFR_INIT_D-358
,アップリンクデータ拡散コード359,0x00000000,0xFFFFFFFF,,0x721CEA8,0x721CEA8,UP_SS_LSFR_INIT_D-359
,アップリンクデータ拡散コード360,0x00000000,0xFFFFFFFF,,0x724988F,0x724988F,UP_SS_LSFR_INIT_D-360
,アップリンクデータ拡散コード361,0x00000000,0xFFFFFFFF,,0x7263BEF,0x7263BEF,UP_SS_LSFR_INIT_D-361
,アップリンクデータ拡散コード362,0x00000000,0xFFFFFFFF,,0x7289037,0x7289037,UP_SS_LSFR_INIT_D-362
,アップリンクデータ拡散コード363,0x00000000,0xFFFFFFFF,,0x72B19E2,0x72B19E2,UP_SS_LSFR_INIT_D-363
,アップリンクデータ拡散コード364,0x00000000,0xFFFFFFFF,,0x72B9DBD,0x72B9DBD,UP_SS_LSFR_INIT_D-364
,アップリンクデータ拡散コード365,0x00000000,0xFFFFFFFF,,0x72C25F7,0x72C25F7,UP_SS_LSFR_INIT_D-365
,アップリンクデータ拡散コード366,0x00000000,0xFFFFFFFF,,0x73170B0,0x73170B0,UP_SS_LSFR_INIT_D-366
,アップリンクデータ拡散コード367,0x00000000,0xFFFFFFFF,,0x7360D6A,0x7360D6A,UP_SS_LSFR_INIT_D-367
,アップリンクデータ拡散コード368,0x00000000,0xFFFFFFFF,,0x73B54F3,0x73B54F3,UP_SS_LSFR_INIT_D-368
,アップリンクデータ拡散コード369,0x00000000,0xFFFFFFFF,,0x73BF37A,0x73BF37A,UP_SS_LSFR_INIT_D-369
,アップリンクデータ拡散コード370,0x00000000,0xFFFFFFFF,,0x741053D,0x741053D,UP_SS_LSFR_INIT_D-370
,アップリンクデータ拡散コード371,0x00000000,0xFFFFFFFF,,0x742AF5A,0x742AF5A,UP_SS_LSFR_INIT_D-371
,アップリンクデータ拡散コード372,0x00000000,0xFFFFFFFF,,0x747E353,0x747E353,UP_SS_LSFR_INIT_D-372
,アップリンクデータ拡散コード373,0x00000000,0xFFFFFFFF,,0x74ACAE7,0x74ACAE7,UP_SS_LSFR_INIT_D-373
,アップリンクデータ拡散コード374,0x00000000,0xFFFFFFFF,,0x74F27E5,0x74F27E5,UP_SS_LSFR_INIT_D-374
,アップリンクデータ拡散コード375,0x00000000,0xFFFFFFFF,,0x74F3FBB,0x74F3FBB,UP_SS_LSFR_INIT_D-375
,アップリンクデータ拡散コード376,0x00000000,0xFFFFFFFF,,0x752E56E,0x752E56E,UP_SS_LSFR_INIT_D-376
,アップリンクデータ拡散コード377,0x00000000,0xFFFFFFFF,,0x75D5E18,0x75D5E18,UP_SS_LSFR_INIT_D-377
,アップリンクデータ拡散コード378,0x00000000,0xFFFFFFFF,,0x7610E67,0x7610E67,UP_SS_LSFR_INIT_D-378
,アップリンクデータ拡散コード379,0x00000000,0xFFFFFFFF,,0x761CFA6,0x761CFA6,UP_SS_LSFR_INIT_D-379
,アップリンクデータ拡散コード380,0x00000000,0xFFFFFFFF,,0x7646175,0x7646175,UP_SS_LSFR_INIT_D-380
,アップリンクデータ拡散コード381,0x00000000,0xFFFFFFFF,,0x76A371F,0x76A371F,UP_SS_LSFR_INIT_D-381
,アップリンクデータ拡散コード382,0x00000000,0xFFFFFFFF,,0x7820134,0x7820134,UP_SS_LSFR_INIT_D-382
,アップリンクデータ拡散コード383,0x00000000,0xFFFFFFFF,,0x78416B5,0x78416B5,UP_SS_LSFR_INIT_D-383
,アップリンクデータ拡散コード384,0x00000000,0xFFFFFFFF,,0x7914C12,0x7914C12,UP_SS_LSFR_INIT_D-384
,アップリンクデータ拡散コード385,0x00000000,0xFFFFFFFF,,0x79F02C2,0x79F02C2,UP_SS_LSFR_INIT_D-385
,アップリンクデータ拡散コード386,0x00000000,0xFFFFFFFF,,0x7A1AD42,0x7A1AD42,UP_SS_LSFR_INIT_D-386
,アップリンクデータ拡散コード387,0x00000000,0xFFFFFFFF,,0x7A6DEB8,0x7A6DEB8,UP_SS_LSFR_INIT_D-387
,アップリンクデータ拡散コード388,0x00000000,0xFFFFFFFF,,0x7AB94AD,0x7AB94AD,UP_SS_LSFR_INIT_D-388
,アップリンクデータ拡散コード389,0x00000000,0xFFFFFFFF,,0x7ABE291,0x7ABE291,UP_SS_LSFR_INIT_D-389
,アップリンクデータ拡散コード390,0x00000000,0xFFFFFFFF,,0x7B06341,0x7B06341,UP_SS_LSFR_INIT_D-390
,アップリンクデータ拡散コード391,0x00000000,0xFFFFFFFF,,0x7C448D6,0x7C448D6,UP_SS_LSFR_INIT_D-391
,アップリンクデータ拡散コード392,0x00000000,0xFFFFFFFF,,0x7CCA694,0x7CCA694,UP_SS_LSFR_INIT_D-392
,アップリンクデータ拡散コード393,0x00000000,0xFFFFFFFF,,0x7D2F6CB,0x7D2F6CB,UP_SS_LSFR_INIT_D-393
,アップリンクデータ拡散コード394,0x00000000,0xFFFFFFFF,,0x7DA73F1,0x7DA73F1,UP_SS_LSFR_INIT_D-394
,アップリンクデータ拡散コード395,0x00000000,0xFFFFFFFF,,0x7DCEBE3,0x7DCEBE3,UP_SS_LSFR_INIT_D-395
,アップリンクデータ拡散コード396,0x00000000,0xFFFFFFFF,,0x7E9CEF6,0x7E9CEF6,UP_SS_LSFR_INIT_D-396
,アップリンクデータ拡散コード397,0x00000000,0xFFFFFFFF,,0x7F03753,0x7F03753,UP_SS_LSFR_INIT_D-397
,アップリンクデータ拡散コード398,0x00000000,0xFFFFFFFF,,0x7F78D51,0x7F78D51,UP_SS_LSFR_INIT_D-398
,アップリンクデータ拡散コード399,0x00000000,0xFFFFFFFF,,0x7FD6372,0x7FD6372,UP_SS_LSFR_INIT_D-399
1741,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x00E6F08,0x00E6F08,UP_SS_LSFR_INIT_P-0
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x015DD2E,0x015DD2E,UP_SS_LSFR_INIT_P-1
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0169478,0x0169478,UP_SS_LSFR_INIT_P-2
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x01A593C,0x01A593C,UP_SS_LSFR_INIT_P-3
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x01CA051,0x01CA051,UP_SS_LSFR_INIT_P-4
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x01D8B4C,0x01D8B4C,UP_SS_LSFR_INIT_P-5
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x024DF6A,0x024DF6A,UP_SS_LSFR_INIT_P-6
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x02923DD,0x02923DD,UP_SS_LSFR_INIT_P-7
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x036CE04,0x036CE04,UP_SS_LSFR_INIT_P-8
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x03C283E,0x03C283E,UP_SS_LSFR_INIT_P-9
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x04091FB,0x04091FB,UP_SS_LSFR_INIT_P-10
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x050AB2E,0x050AB2E,UP_SS_LSFR_INIT_P-11
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x05314B7,0x05314B7,UP_SS_LSFR_INIT_P-12
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x058C2BB,0x058C2BB,UP_SS_LSFR_INIT_P-13
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x059CF19,0x059CF19,UP_SS_LSFR_INIT_P-14
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x05BE6A4,0x05BE6A4,UP_SS_LSFR_INIT_P-15
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x05FD059,0x05FD059,UP_SS_LSFR_INIT_P-16
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x063CF77,0x063CF77,UP_SS_LSFR_INIT_P-17
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x064105D,0x064105D,UP_SS_LSFR_INIT_P-18
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x064A5AD,0x064A5AD,UP_SS_LSFR_INIT_P-19
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x06892B3,0x06892B3,UP_SS_LSFR_INIT_P-20
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0694FF5,0x0694FF5,UP_SS_LSFR_INIT_P-21
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x06FDA9D,0x06FDA9D,UP_SS_LSFR_INIT_P-22
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x070CC76,0x070CC76,UP_SS_LSFR_INIT_P-23
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x07BA948,0x07BA948,UP_SS_LSFR_INIT_P-24
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x07E6BA3,0x07E6BA3,UP_SS_LSFR_INIT_P-25
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x083EC78,0x083EC78,UP_SS_LSFR_INIT_P-26
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0842040,0x0842040,UP_SS_LSFR_INIT_P-27
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0888452,0x0888452,UP_SS_LSFR_INIT_P-28
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x089897F,0x089897F,UP_SS_LSFR_INIT_P-29
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x089B076,0x089B076,UP_SS_LSFR_INIT_P-30
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x08C1352,0x08C1352,UP_SS_LSFR_INIT_P-31
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0905D78,0x0905D78,UP_SS_LSFR_INIT_P-32
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x09D52D7,0x09D52D7,UP_SS_LSFR_INIT_P-33
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x09DE238,0x09DE238,UP_SS_LSFR_INIT_P-34
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0A121D7,0x0A121D7,UP_SS_LSFR_INIT_P-35
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0A417DD,0x0A417DD,UP_SS_LSFR_INIT_P-36
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0A7C0BC,0x0A7C0BC,UP_SS_LSFR_INIT_P-37
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0ADA9B3,0x0ADA9B3,UP_SS_LSFR_INIT_P-38
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0D29722,0x0D29722,UP_SS_LSFR_INIT_P-39
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0DB5A47,0x0DB5A47,UP_SS_LSFR_INIT_P-40
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0DCB122,0x0DCB122,UP_SS_LSFR_INIT_P-41
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0E49594,0x0E49594,UP_SS_LSFR_INIT_P-42
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x0EB6637,0x0EB6637,UP_SS_LSFR_INIT_P-43
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x103B89E,0x103B89E,UP_SS_LSFR_INIT_P-44
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x119CF95,0x119CF95,UP_SS_LSFR_INIT_P-45
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1220261,0x1220261,UP_SS_LSFR_INIT_P-46
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x12B9203,0x12B9203,UP_SS_LSFR_INIT_P-47
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x139C4AB,0x139C4AB,UP_SS_LSFR_INIT_P-48
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x13A52B8,0x13A52B8,UP_SS_LSFR_INIT_P-49
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x13CAA09,0x13CAA09,UP_SS_LSFR_INIT_P-50
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x13EE342,0x13EE342,UP_SS_LSFR_INIT_P-51
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1423F78,0x1423F78,UP_SS_LSFR_INIT_P-52
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1426438,0x1426438,UP_SS_LSFR_INIT_P-53
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x142C08F,0x142C08F,UP_SS_LSFR_INIT_P-54
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1471815,0x1471815,UP_SS_LSFR_INIT_P-55
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1496BC7,0x1496BC7,UP_SS_LSFR_INIT_P-56
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x14A56DC,0x14A56DC,UP_SS_LSFR_INIT_P-57
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1560CA7,0x1560CA7,UP_SS_LSFR_INIT_P-58
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x156B591,0x156B591,UP_SS_LSFR_INIT_P-59
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x15C70F8,0x15C70F8,UP_SS_LSFR_INIT_P-60
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x162A23E,0x162A23E,UP_SS_LSFR_INIT_P-61
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x16567AA,0x16567AA,UP_SS_LSFR_INIT_P-62
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x16E381F,0x16E381F,UP_SS_LSFR_INIT_P-63
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1721DC2,0x1721DC2,UP_SS_LSFR_INIT_P-64
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1768690,0x1768690,UP_SS_LSFR_INIT_P-65
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x17A1B39,0x17A1B39,UP_SS_LSFR_INIT_P-66
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x17A6EF3,0x17A6EF3,UP_SS_LSFR_INIT_P-67
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x18AADD0,0x18AADD0,UP_SS_LSFR_INIT_P-68
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x195ED65,0x195ED65,UP_SS_LSFR_INIT_P-69
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x19C5884,0x19C5884,UP_SS_LSFR_INIT_P-70
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x19ED637,0x19ED637,UP_SS_LSFR_INIT_P-71
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1AAA124,0x1AAA124,UP_SS_LSFR_INIT_P-72
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1B3FC3E,0x1B3FC3E,UP_SS_LSFR_INIT_P-73
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1B86F25,0x1B86F25,UP_SS_LSFR_INIT_P-74
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1BCC4E6,0x1BCC4E6,UP_SS_LSFR_INIT_P-75
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1BD5B58,0x1BD5B58,UP_SS_LSFR_INIT_P-76
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1C22370,0x1C22370,UP_SS_LSFR_INIT_P-77
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1C73507,0x1C73507,UP_SS_LSFR_INIT_P-78
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1C92A88,0x1C92A88,UP_SS_LSFR_INIT_P-79
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1CACA55,0x1CACA55,UP_SS_LSFR_INIT_P-80
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1CC3CD0,0x1CC3CD0,UP_SS_LSFR_INIT_P-81
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1DADC43,0x1DADC43,UP_SS_LSFR_INIT_P-82
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1E4C836,0x1E4C836,UP_SS_LSFR_INIT_P-83
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1E74713,0x1E74713,UP_SS_LSFR_INIT_P-84
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1E7F4D6,0x1E7F4D6,UP_SS_LSFR_INIT_P-85
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1ECDB36,0x1ECDB36,UP_SS_LSFR_INIT_P-86
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1EFA9AD,0x1EFA9AD,UP_SS_LSFR_INIT_P-87
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1F89569,0x1F89569,UP_SS_LSFR_INIT_P-88
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x1F9F401,0x1F9F401,UP_SS_LSFR_INIT_P-89
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2095195,0x2095195,UP_SS_LSFR_INIT_P-90
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x20CC165,0x20CC165,UP_SS_LSFR_INIT_P-91
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x21247A6,0x21247A6,UP_SS_LSFR_INIT_P-92
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2179C67,0x2179C67,UP_SS_LSFR_INIT_P-93
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x21B68F0,0x21B68F0,UP_SS_LSFR_INIT_P-94
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x21CB9E2,0x21CB9E2,UP_SS_LSFR_INIT_P-95
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x21D7B99,0x21D7B99,UP_SS_LSFR_INIT_P-96
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x21F41B7,0x21F41B7,UP_SS_LSFR_INIT_P-97
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2241019,0x2241019,UP_SS_LSFR_INIT_P-98
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x22692BD,0x22692BD,UP_SS_LSFR_INIT_P-99
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x22A99D5,0x22A99D5,UP_SS_LSFR_INIT_P-100
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x22EF79C,0x22EF79C,UP_SS_LSFR_INIT_P-101
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2328A81,0x2328A81,UP_SS_LSFR_INIT_P-102
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x23417F9,0x23417F9,UP_SS_LSFR_INIT_P-103
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x24B9496,0x24B9496,UP_SS_LSFR_INIT_P-104
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x258ADBF,0x258ADBF,UP_SS_LSFR_INIT_P-105
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x25B55DE,0x25B55DE,UP_SS_LSFR_INIT_P-106
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x25BE3B9,0x25BE3B9,UP_SS_LSFR_INIT_P-107
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x25C0769,0x25C0769,UP_SS_LSFR_INIT_P-108
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x266A431,0x266A431,UP_SS_LSFR_INIT_P-109
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x266D74D,0x266D74D,UP_SS_LSFR_INIT_P-110
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x26C2D24,0x26C2D24,UP_SS_LSFR_INIT_P-111
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x26D1876,0x26D1876,UP_SS_LSFR_INIT_P-112
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x26DD6BB,0x26DD6BB,UP_SS_LSFR_INIT_P-113
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2730060,0x2730060,UP_SS_LSFR_INIT_P-114
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x28A2A68,0x28A2A68,UP_SS_LSFR_INIT_P-115
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x28E11C1,0x28E11C1,UP_SS_LSFR_INIT_P-116
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2922C92,0x2922C92,UP_SS_LSFR_INIT_P-117
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x294C6A1,0x294C6A1,UP_SS_LSFR_INIT_P-118
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2963B8D,0x2963B8D,UP_SS_LSFR_INIT_P-119
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A22BB0,0x2A22BB0,UP_SS_LSFR_INIT_P-120
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A60810,0x2A60810,UP_SS_LSFR_INIT_P-121
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A6BC94,0x2A6BC94,UP_SS_LSFR_INIT_P-122
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A81F42,0x2A81F42,UP_SS_LSFR_INIT_P-123
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A91EC0,0x2A91EC0,UP_SS_LSFR_INIT_P-124
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2A9A1E6,0x2A9A1E6,UP_SS_LSFR_INIT_P-125
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2ADE9B8,0x2ADE9B8,UP_SS_LSFR_INIT_P-126
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2B03AAC,0x2B03AAC,UP_SS_LSFR_INIT_P-127
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2B22352,0x2B22352,UP_SS_LSFR_INIT_P-128
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2B991B8,0x2B991B8,UP_SS_LSFR_INIT_P-129
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2C12ED6,0x2C12ED6,UP_SS_LSFR_INIT_P-130
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2C26F07,0x2C26F07,UP_SS_LSFR_INIT_P-131
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2CFC664,0x2CFC664,UP_SS_LSFR_INIT_P-132
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2D6615E,0x2D6615E,UP_SS_LSFR_INIT_P-133
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2E027EB,0x2E027EB,UP_SS_LSFR_INIT_P-134
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2E149C1,0x2E149C1,UP_SS_LSFR_INIT_P-135
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2FA5203,0x2FA5203,UP_SS_LSFR_INIT_P-136
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2FBAA19,0x2FBAA19,UP_SS_LSFR_INIT_P-137
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2FD569E,0x2FD569E,UP_SS_LSFR_INIT_P-138
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x2FD5B3C,0x2FD5B3C,UP_SS_LSFR_INIT_P-139
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x300EF3F,0x300EF3F,UP_SS_LSFR_INIT_P-140
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x309A623,0x309A623,UP_SS_LSFR_INIT_P-141
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x30C335A,0x30C335A,UP_SS_LSFR_INIT_P-142
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x30D48D0,0x30D48D0,UP_SS_LSFR_INIT_P-143
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x30E4ED4,0x30E4ED4,UP_SS_LSFR_INIT_P-144
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x30E5941,0x30E5941,UP_SS_LSFR_INIT_P-145
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x310B4B3,0x310B4B3,UP_SS_LSFR_INIT_P-146
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3116E5D,0x3116E5D,UP_SS_LSFR_INIT_P-147
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x31A2D93,0x31A2D93,UP_SS_LSFR_INIT_P-148
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x31C9B88,0x31C9B88,UP_SS_LSFR_INIT_P-149
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3210646,0x3210646,UP_SS_LSFR_INIT_P-150
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3291767,0x3291767,UP_SS_LSFR_INIT_P-151
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x32AB4A6,0x32AB4A6,UP_SS_LSFR_INIT_P-152
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x32DD49E,0x32DD49E,UP_SS_LSFR_INIT_P-153
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x32F1198,0x32F1198,UP_SS_LSFR_INIT_P-154
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x33328EC,0x33328EC,UP_SS_LSFR_INIT_P-155
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x335E7C2,0x335E7C2,UP_SS_LSFR_INIT_P-156
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x33C34F8,0x33C34F8,UP_SS_LSFR_INIT_P-157
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x33D3BE4,0x33D3BE4,UP_SS_LSFR_INIT_P-158
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x34A2A81,0x34A2A81,UP_SS_LSFR_INIT_P-159
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3514387,0x3514387,UP_SS_LSFR_INIT_P-160
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x35221A3,0x35221A3,UP_SS_LSFR_INIT_P-161
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x35396A9,0x35396A9,UP_SS_LSFR_INIT_P-162
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x354BBD8,0x354BBD8,UP_SS_LSFR_INIT_P-163
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x357E847,0x357E847,UP_SS_LSFR_INIT_P-164
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x358A7B2,0x358A7B2,UP_SS_LSFR_INIT_P-165
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x35AEE8D,0x35AEE8D,UP_SS_LSFR_INIT_P-166
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3607C5B,0x3607C5B,UP_SS_LSFR_INIT_P-167
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x360A76A,0x360A76A,UP_SS_LSFR_INIT_P-168
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x369917C,0x369917C,UP_SS_LSFR_INIT_P-169
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x372D064,0x372D064,UP_SS_LSFR_INIT_P-170
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x374DA9C,0x374DA9C,UP_SS_LSFR_INIT_P-171
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x37589CA,0x37589CA,UP_SS_LSFR_INIT_P-172
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x37ABA2A,0x37ABA2A,UP_SS_LSFR_INIT_P-173
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x37E991C,0x37E991C,UP_SS_LSFR_INIT_P-174
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3854119,0x3854119,UP_SS_LSFR_INIT_P-175
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x38E3F14,0x38E3F14,UP_SS_LSFR_INIT_P-176
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3954378,0x3954378,UP_SS_LSFR_INIT_P-177
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x39DAB9E,0x39DAB9E,UP_SS_LSFR_INIT_P-178
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3A380CD,0x3A380CD,UP_SS_LSFR_INIT_P-179
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3A8558D,0x3A8558D,UP_SS_LSFR_INIT_P-180
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3AA187A,0x3AA187A,UP_SS_LSFR_INIT_P-181
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3AB8511,0x3AB8511,UP_SS_LSFR_INIT_P-182
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3AD8077,0x3AD8077,UP_SS_LSFR_INIT_P-183
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3AE1870,0x3AE1870,UP_SS_LSFR_INIT_P-184
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3B66721,0x3B66721,UP_SS_LSFR_INIT_P-185
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3BB70AD,0x3BB70AD,UP_SS_LSFR_INIT_P-186
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3C7204E,0x3C7204E,UP_SS_LSFR_INIT_P-187
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3CAFC88,0x3CAFC88,UP_SS_LSFR_INIT_P-188
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3CBF1BF,0x3CBF1BF,UP_SS_LSFR_INIT_P-189
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3CC16EA,0x3CC16EA,UP_SS_LSFR_INIT_P-190
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3D6B23C,0x3D6B23C,UP_SS_LSFR_INIT_P-191
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3D72D53,0x3D72D53,UP_SS_LSFR_INIT_P-192
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3DF6CC9,0x3DF6CC9,UP_SS_LSFR_INIT_P-193
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3E6C6FB,0x3E6C6FB,UP_SS_LSFR_INIT_P-194
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3E8A63A,0x3E8A63A,UP_SS_LSFR_INIT_P-195
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3F7B773,0x3F7B773,UP_SS_LSFR_INIT_P-196
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x3F90B37,0x3F90B37,UP_SS_LSFR_INIT_P-197
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x409E9A2,0x409E9A2,UP_SS_LSFR_INIT_P-198
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x40F51DF,0x40F51DF,UP_SS_LSFR_INIT_P-199
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x40FD3A5,0x40FD3A5,UP_SS_LSFR_INIT_P-200
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x41216B0,0x41216B0,UP_SS_LSFR_INIT_P-201
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4141AB3,0x4141AB3,UP_SS_LSFR_INIT_P-202
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x415C287,0x415C287,UP_SS_LSFR_INIT_P-203
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4247577,0x4247577,UP_SS_LSFR_INIT_P-204
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4255C96,0x4255C96,UP_SS_LSFR_INIT_P-205
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x427B419,0x427B419,UP_SS_LSFR_INIT_P-206
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4280FDA,0x4280FDA,UP_SS_LSFR_INIT_P-207
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x428B3A8,0x428B3A8,UP_SS_LSFR_INIT_P-208
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x42BB6BC,0x42BB6BC,UP_SS_LSFR_INIT_P-209
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x42F9F0C,0x42F9F0C,UP_SS_LSFR_INIT_P-210
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x43ABEDD,0x43ABEDD,UP_SS_LSFR_INIT_P-211
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x43DCE66,0x43DCE66,UP_SS_LSFR_INIT_P-212
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x44064A9,0x44064A9,UP_SS_LSFR_INIT_P-213
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x44535A1,0x44535A1,UP_SS_LSFR_INIT_P-214
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x45157E1,0x45157E1,UP_SS_LSFR_INIT_P-215
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4524746,0x4524746,UP_SS_LSFR_INIT_P-216
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x45BF912,0x45BF912,UP_SS_LSFR_INIT_P-217
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x45E6A8C,0x45E6A8C,UP_SS_LSFR_INIT_P-218
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4603544,0x4603544,UP_SS_LSFR_INIT_P-219
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x460E7F6,0x460E7F6,UP_SS_LSFR_INIT_P-220
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4627262,0x4627262,UP_SS_LSFR_INIT_P-221
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4690252,0x4690252,UP_SS_LSFR_INIT_P-222
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x471891E,0x471891E,UP_SS_LSFR_INIT_P-223
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4720096,0x4720096,UP_SS_LSFR_INIT_P-224
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4720910,0x4720910,UP_SS_LSFR_INIT_P-225
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x47756E0,0x47756E0,UP_SS_LSFR_INIT_P-226
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x47E095A,0x47E095A,UP_SS_LSFR_INIT_P-227
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x47EEE07,0x47EEE07,UP_SS_LSFR_INIT_P-228
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x482A355,0x482A355,UP_SS_LSFR_INIT_P-229
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x48E3E45,0x48E3E45,UP_SS_LSFR_INIT_P-230
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x48F05CC,0x48F05CC,UP_SS_LSFR_INIT_P-231
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x49122D7,0x49122D7,UP_SS_LSFR_INIT_P-232
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4930706,0x4930706,UP_SS_LSFR_INIT_P-233
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4934CC8,0x4934CC8,UP_SS_LSFR_INIT_P-234
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x494606F,0x494606F,UP_SS_LSFR_INIT_P-235
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x497FD1D,0x497FD1D,UP_SS_LSFR_INIT_P-236
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x49F6EB9,0x49F6EB9,UP_SS_LSFR_INIT_P-237
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4AEABD3,0x4AEABD3,UP_SS_LSFR_INIT_P-238
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4AFAE22,0x4AFAE22,UP_SS_LSFR_INIT_P-239
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4B1D9FC,0x4B1D9FC,UP_SS_LSFR_INIT_P-240
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4B8FDF7,0x4B8FDF7,UP_SS_LSFR_INIT_P-241
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4BE2D01,0x4BE2D01,UP_SS_LSFR_INIT_P-242
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4CDF7E2,0x4CDF7E2,UP_SS_LSFR_INIT_P-243
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4CE99DF,0x4CE99DF,UP_SS_LSFR_INIT_P-244
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4CEED26,0x4CEED26,UP_SS_LSFR_INIT_P-245
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4D0E901,0x4D0E901,UP_SS_LSFR_INIT_P-246
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4D7D492,0x4D7D492,UP_SS_LSFR_INIT_P-247
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4E68CCF,0x4E68CCF,UP_SS_LSFR_INIT_P-248
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4F295A5,0x4F295A5,UP_SS_LSFR_INIT_P-249
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4F73DF8,0x4F73DF8,UP_SS_LSFR_INIT_P-250
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4FBABBD,0x4FBABBD,UP_SS_LSFR_INIT_P-251
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4FC1F9D,0x4FC1F9D,UP_SS_LSFR_INIT_P-252
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x4FE4269,0x4FE4269,UP_SS_LSFR_INIT_P-253
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5007C60,0x5007C60,UP_SS_LSFR_INIT_P-254
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5020E0C,0x5020E0C,UP_SS_LSFR_INIT_P-255
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x50C3582,0x50C3582,UP_SS_LSFR_INIT_P-256
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5175A64,0x5175A64,UP_SS_LSFR_INIT_P-257
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x520867E,0x520867E,UP_SS_LSFR_INIT_P-258
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x52E0BDF,0x52E0BDF,UP_SS_LSFR_INIT_P-259
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x52EB835,0x52EB835,UP_SS_LSFR_INIT_P-260
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x530CAF6,0x530CAF6,UP_SS_LSFR_INIT_P-261
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x534E1B4,0x534E1B4,UP_SS_LSFR_INIT_P-262
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x53758B1,0x53758B1,UP_SS_LSFR_INIT_P-263
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x543ADD5,0x543ADD5,UP_SS_LSFR_INIT_P-264
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5504A4A,0x5504A4A,UP_SS_LSFR_INIT_P-265
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5540235,0x5540235,UP_SS_LSFR_INIT_P-266
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x55AD5F7,0x55AD5F7,UP_SS_LSFR_INIT_P-267
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x55B1DE6,0x55B1DE6,UP_SS_LSFR_INIT_P-268
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x55DD1F7,0x55DD1F7,UP_SS_LSFR_INIT_P-269
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5610471,0x5610471,UP_SS_LSFR_INIT_P-270
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x563618F,0x563618F,UP_SS_LSFR_INIT_P-271
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x563D363,0x563D363,UP_SS_LSFR_INIT_P-272
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x565433E,0x565433E,UP_SS_LSFR_INIT_P-273
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5723117,0x5723117,UP_SS_LSFR_INIT_P-274
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5746579,0x5746579,UP_SS_LSFR_INIT_P-275
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x57B065B,0x57B065B,UP_SS_LSFR_INIT_P-276
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x588D565,0x588D565,UP_SS_LSFR_INIT_P-277
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5890E7B,0x5890E7B,UP_SS_LSFR_INIT_P-278
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x59C9DC8,0x59C9DC8,UP_SS_LSFR_INIT_P-279
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5A23E33,0x5A23E33,UP_SS_LSFR_INIT_P-280
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5A51AF7,0x5A51AF7,UP_SS_LSFR_INIT_P-281
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5A65795,0x5A65795,UP_SS_LSFR_INIT_P-282
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5A99F4D,0x5A99F4D,UP_SS_LSFR_INIT_P-283
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5AA4318,0x5AA4318,UP_SS_LSFR_INIT_P-284
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5AE117F,0x5AE117F,UP_SS_LSFR_INIT_P-285
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5AED469,0x5AED469,UP_SS_LSFR_INIT_P-286
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5B20A78,0x5B20A78,UP_SS_LSFR_INIT_P-287
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5BCAA6B,0x5BCAA6B,UP_SS_LSFR_INIT_P-288
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5BE43CF,0x5BE43CF,UP_SS_LSFR_INIT_P-289
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5CBDF6B,0x5CBDF6B,UP_SS_LSFR_INIT_P-290
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5DFEB66,0x5DFEB66,UP_SS_LSFR_INIT_P-291
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5E20EE6,0x5E20EE6,UP_SS_LSFR_INIT_P-292
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5E48D76,0x5E48D76,UP_SS_LSFR_INIT_P-293
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5E56DBE,0x5E56DBE,UP_SS_LSFR_INIT_P-294
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5E5AEFE,0x5E5AEFE,UP_SS_LSFR_INIT_P-295
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5F651A4,0x5F651A4,UP_SS_LSFR_INIT_P-296
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5F99DD2,0x5F99DD2,UP_SS_LSFR_INIT_P-297
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5FA61A9,0x5FA61A9,UP_SS_LSFR_INIT_P-298
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5FD1626,0x5FD1626,UP_SS_LSFR_INIT_P-299
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5FD2279,0x5FD2279,UP_SS_LSFR_INIT_P-300
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x5FEC596,0x5FEC596,UP_SS_LSFR_INIT_P-301
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x60EB484,0x60EB484,UP_SS_LSFR_INIT_P-302
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6104B26,0x6104B26,UP_SS_LSFR_INIT_P-303
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x611506A,0x611506A,UP_SS_LSFR_INIT_P-304
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x61263F4,0x61263F4,UP_SS_LSFR_INIT_P-305
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x615C163,0x615C163,UP_SS_LSFR_INIT_P-306
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x616AE54,0x616AE54,UP_SS_LSFR_INIT_P-307
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x622124C,0x622124C,UP_SS_LSFR_INIT_P-308
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x627F8AD,0x627F8AD,UP_SS_LSFR_INIT_P-309
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x62853FD,0x62853FD,UP_SS_LSFR_INIT_P-310
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6317A02,0x6317A02,UP_SS_LSFR_INIT_P-311
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6349060,0x6349060,UP_SS_LSFR_INIT_P-312
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x63B19FB,0x63B19FB,UP_SS_LSFR_INIT_P-313
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6508199,0x6508199,UP_SS_LSFR_INIT_P-314
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6534230,0x6534230,UP_SS_LSFR_INIT_P-315
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x657350C,0x657350C,UP_SS_LSFR_INIT_P-316
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x65B983E,0x65B983E,UP_SS_LSFR_INIT_P-317
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x65E949A,0x65E949A,UP_SS_LSFR_INIT_P-318
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x666A16B,0x666A16B,UP_SS_LSFR_INIT_P-319
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6676493,0x6676493,UP_SS_LSFR_INIT_P-320
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x66BFA0D,0x66BFA0D,UP_SS_LSFR_INIT_P-321
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x66CAC25,0x66CAC25,UP_SS_LSFR_INIT_P-322
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x66DDDA6,0x66DDDA6,UP_SS_LSFR_INIT_P-323
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x67C453A,0x67C453A,UP_SS_LSFR_INIT_P-324
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6819D32,0x6819D32,UP_SS_LSFR_INIT_P-325
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6839FBF,0x6839FBF,UP_SS_LSFR_INIT_P-326
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x684BB82,0x684BB82,UP_SS_LSFR_INIT_P-327
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x68B9397,0x68B9397,UP_SS_LSFR_INIT_P-328
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x69B4CA7,0x69B4CA7,UP_SS_LSFR_INIT_P-329
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x69CE272,0x69CE272,UP_SS_LSFR_INIT_P-330
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x69D2348,0x69D2348,UP_SS_LSFR_INIT_P-331
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6A46F11,0x6A46F11,UP_SS_LSFR_INIT_P-332
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6ABA182,0x6ABA182,UP_SS_LSFR_INIT_P-333
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6AD3004,0x6AD3004,UP_SS_LSFR_INIT_P-334
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6AE1445,0x6AE1445,UP_SS_LSFR_INIT_P-335
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6B015D0,0x6B015D0,UP_SS_LSFR_INIT_P-336
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6B3B5EF,0x6B3B5EF,UP_SS_LSFR_INIT_P-337
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6B3D60D,0x6B3D60D,UP_SS_LSFR_INIT_P-338
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6B6CD2E,0x6B6CD2E,UP_SS_LSFR_INIT_P-339
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6BA21AA,0x6BA21AA,UP_SS_LSFR_INIT_P-340
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6BADCB9,0x6BADCB9,UP_SS_LSFR_INIT_P-341
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6BE319C,0x6BE319C,UP_SS_LSFR_INIT_P-342
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6C33AC5,0x6C33AC5,UP_SS_LSFR_INIT_P-343
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6C3C8B7,0x6C3C8B7,UP_SS_LSFR_INIT_P-344
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6D7DFE1,0x6D7DFE1,UP_SS_LSFR_INIT_P-345
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6D8E579,0x6D8E579,UP_SS_LSFR_INIT_P-346
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6DE9FE1,0x6DE9FE1,UP_SS_LSFR_INIT_P-347
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6DF6B65,0x6DF6B65,UP_SS_LSFR_INIT_P-348
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6E0111F,0x6E0111F,UP_SS_LSFR_INIT_P-349
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6E6594B,0x6E6594B,UP_SS_LSFR_INIT_P-350
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6F1D5DE,0x6F1D5DE,UP_SS_LSFR_INIT_P-351
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6F62224,0x6F62224,UP_SS_LSFR_INIT_P-352
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6F77429,0x6F77429,UP_SS_LSFR_INIT_P-353
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6F8852A,0x6F8852A,UP_SS_LSFR_INIT_P-354
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x6FCEEFB,0x6FCEEFB,UP_SS_LSFR_INIT_P-355
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x70C2F03,0x70C2F03,UP_SS_LSFR_INIT_P-356
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x710671C,0x710671C,UP_SS_LSFR_INIT_P-357
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x719235E,0x719235E,UP_SS_LSFR_INIT_P-358
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x71F6C36,0x71F6C36,UP_SS_LSFR_INIT_P-359
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7217A61,0x7217A61,UP_SS_LSFR_INIT_P-360
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x725BF2C,0x725BF2C,UP_SS_LSFR_INIT_P-361
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7267758,0x7267758,UP_SS_LSFR_INIT_P-362
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7281D50,0x7281D50,UP_SS_LSFR_INIT_P-363
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x728390A,0x728390A,UP_SS_LSFR_INIT_P-364
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x73F146B,0x73F146B,UP_SS_LSFR_INIT_P-365
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7463C51,0x7463C51,UP_SS_LSFR_INIT_P-366
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x74CBB4D,0x74CBB4D,UP_SS_LSFR_INIT_P-367
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7506D21,0x7506D21,UP_SS_LSFR_INIT_P-368
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x75395EB,0x75395EB,UP_SS_LSFR_INIT_P-369
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x754156B,0x754156B,UP_SS_LSFR_INIT_P-370
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x76083B8,0x76083B8,UP_SS_LSFR_INIT_P-371
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7621C99,0x7621C99,UP_SS_LSFR_INIT_P-372
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x76A0A57,0x76A0A57,UP_SS_LSFR_INIT_P-373
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x76CE270,0x76CE270,UP_SS_LSFR_INIT_P-374
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7745B8D,0x7745B8D,UP_SS_LSFR_INIT_P-375
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7783B5D,0x7783B5D,UP_SS_LSFR_INIT_P-376
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7796E14,0x7796E14,UP_SS_LSFR_INIT_P-377
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x77CBDEE,0x77CBDEE,UP_SS_LSFR_INIT_P-378
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7803FC5,0x7803FC5,UP_SS_LSFR_INIT_P-379
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x786C646,0x786C646,UP_SS_LSFR_INIT_P-380
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x78783A7,0x78783A7,UP_SS_LSFR_INIT_P-381
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x794ECCD,0x794ECCD,UP_SS_LSFR_INIT_P-382
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x798066F,0x798066F,UP_SS_LSFR_INIT_P-383
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x79B3AC9,0x79B3AC9,UP_SS_LSFR_INIT_P-384
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7A60896,0x7A60896,UP_SS_LSFR_INIT_P-385
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7AA1E97,0x7AA1E97,UP_SS_LSFR_INIT_P-386
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7B27DF4,0x7B27DF4,UP_SS_LSFR_INIT_P-387
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7C26960,0x7C26960,UP_SS_LSFR_INIT_P-388
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7CA98CE,0x7CA98CE,UP_SS_LSFR_INIT_P-389
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7D5D3AF,0x7D5D3AF,UP_SS_LSFR_INIT_P-390
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7DA1935,0x7DA1935,UP_SS_LSFR_INIT_P-391
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7DA50A2,0x7DA50A2,UP_SS_LSFR_INIT_P-392
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7E0300E,0x7E0300E,UP_SS_LSFR_INIT_P-393
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7E1F001,0x7E1F001,UP_SS_LSFR_INIT_P-394
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7E7B3BC,0x7E7B3BC,UP_SS_LSFR_INIT_P-395
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7F17FBF,0x7F17FBF,UP_SS_LSFR_INIT_P-396
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7F91F4D,0x7F91F4D,UP_SS_LSFR_INIT_P-397
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7FCF9F8,0x7FCF9F8,UP_SS_LSFR_INIT_P-398
,アップリンクプリアンブル拡散コード（LSFR初期値）,0x00000000,0xFFFFFFFF,,0x7FF68B7,0x7FF68B7,UP_SS_LSFR_INIT_P-399
1742,温度センサーオフセット,-1000,1000,0.1℃,0,0,VND_Temp_Mon_Offset
1743,RSSI bit weight,-30000,30000,0.0001mV/LSB,1544,1544,VND_RSSI_bit_weight
1744,RSSIの更新周期,1000,10000,ms,1000,1000,VND_RSSI_CycleTime
1746,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_0-[0]
1747,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_0-[1]
1748,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_0-[2]
1749,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_0-[3]
1750,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_0-[4]
1751,PLL Synthesizer　0ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x320000,,VND_PLL_Syn_0-[5]
1752,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_1-[0]
1753,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_1-[1]
1754,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_1-[2]
1755,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_1-[3]
1756,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_1-[4]
1757,PLL Synthesizer　1ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x360000,,VND_PLL_Syn_1-[5]
1758,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_2-[0]
1759,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_2-[1]
1760,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_2-[2]
1761,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_2-[3]
1762,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_2-[4]
1763,PLL Synthesizer　2ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x3A0000,,VND_PLL_Syn_2-[5]
1764,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_3-[0]
1765,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_3-[1]
1766,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_3-[2]
1767,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_3-[3]
1768,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_3-[4]
1769,PLL Synthesizer　3ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x3E0000,,VND_PLL_Syn_3-[5]
1770,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_4-[0]
1771,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_4-[1]
1772,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_4-[2]
1773,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_4-[3]
1774,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_4-[4]
1775,PLL Synthesizer　4ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x420000,,VND_PLL_Syn_4-[5]
1776,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_5-[0]
1777,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_5-[1]
1778,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_5-[2]
1779,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_5-[3]
1780,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_5-[4]
1781,PLL Synthesizer　5ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x460000,,VND_PLL_Syn_5-[5]
1782,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_6-[0]
1783,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_6-[1]
1784,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_6-[2]
1785,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_6-[3]
1786,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_6-[4]
1787,PLL Synthesizer　6ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x4A0000,,VND_PLL_Syn_6-[5]
1788,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_7-[0]
1789,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_7-[1]
1790,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_7-[2]
1791,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_7-[3]
1792,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_7-[4]
1793,PLL Synthesizer　7ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x4E0000,,VND_PLL_Syn_7-[5]
1794,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_8-[0]
1795,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_8-[1]
1796,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_8-[2]
1797,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_8-[3]
1798,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_8-[4]
1799,PLL Synthesizer　8ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x520000,,VND_PLL_Syn_8-[5]
1800,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_9-[0]
1801,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_9-[1]
1802,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_9-[2]
1803,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_9-[3]
1804,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_9-[4]
1805,PLL Synthesizer　9ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x560000,,VND_PLL_Syn_9-[5]
1806,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_10-[0]
1807,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_10-[1]
1808,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_10-[2]
1809,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_10-[3]
1810,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_10-[4]
1811,PLL Synthesizer　10ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x5A0000,,VND_PLL_Syn_10-[5]
1812,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_11-[0]
1813,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_11-[1]
1814,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_11-[2]
1815,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_11-[3]
1816,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_11-[4]
1817,PLL Synthesizer　11ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x5E0000,,VND_PLL_Syn_11-[5]
1818,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_12-[0]
1819,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_12-[1]
1820,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_12-[2]
1821,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_12-[3]
1822,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_12-[4]
1823,PLL Synthesizer　12ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x620000,,VND_PLL_Syn_12-[5]
1824,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000001,,VND_PLL_Syn_13-[0]
1825,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x202F4A,,VND_PLL_Syn_13-[1]
1826,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x501E3C,,VND_PLL_Syn_13-[2]
1827,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000005,,VND_PLL_Syn_13-[3]
1828,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x000068,,VND_PLL_Syn_13-[4]
1829,PLL Synthesizer　13ch　レジスタ設定値,0x000000,0xFFFFFF,－－－,0x660000,,VND_PLL_Syn_13-[5]
1829,General【設定③】　レジスタ設定値,0x000000,0x003FFF,,0x00034D,,VND_PLL_Gen3[0]
1830,General【設定③】　レジスタ設定値,0x000000,0x0001FF,,0x0000C1,,VND_PLL_Gen3[1]";
    }
}
