// blowfish.cpp   C++ class implementation of the BLOWFISH encryption algorithm
// _THE BLOWFISH ENCRYPTION ALGORITHM_
// by Bruce Schneier
// Revised code--3/20/94
// Converted to C++ class 5/96, Jim Conger

#include "blowfish.h"
#include <stdlib.h>
#include <arpa/inet.h>

#define S(x,i) (SBoxes[i][x.w.byte##i])
#define bf_F(x) (((S(x,0) + S(x,1)) ^ S(x,2)) + S(x,3))
#define ROUND(a,b,n) (a.dword ^= bf_F(b) ^ PArray[n])

DWORD   * PArray ;
//DWORD   (* SBoxes)[256];
DWORD   ** SBoxes;

void Decstractor(){
    //delete PArray ;
    //delete [] SBoxes ;
    
    free(PArray);
    PArray = NULL;
    
    free(SBoxes);
    SBoxes = NULL;
}

// the low level (private) encryption function
void Blowfish_encipher (DWORD *xl, DWORD *xr)
{
    union aword  Xl, Xr ;
    
    Xl.dword = *xl ;
    Xr.dword = *xr ;
    
    Xl.dword ^= PArray [0];
    ROUND (Xr, Xl, 1) ;  ROUND (Xl, Xr, 2) ;
    ROUND (Xr, Xl, 3) ;  ROUND (Xl, Xr, 4) ;
    ROUND (Xr, Xl, 5) ;  ROUND (Xl, Xr, 6) ;
    ROUND (Xr, Xl, 7) ;  ROUND (Xl, Xr, 8) ;
    ROUND (Xr, Xl, 9) ;  ROUND (Xl, Xr, 10) ;
    ROUND (Xr, Xl, 11) ; ROUND (Xl, Xr, 12) ;
    ROUND (Xr, Xl, 13) ; ROUND (Xl, Xr, 14) ;
    ROUND (Xr, Xl, 15) ; ROUND (Xl, Xr, 16) ;
    Xr.dword ^= PArray [17] ;
    
    *xr = Xl.dword ;
    *xl = Xr.dword ;
}

// the low level (private) decryption function
void Blowfish_decipher (DWORD *xl, DWORD *xr)
{
    union aword  Xl ;
    union aword  Xr ;
    
    Xl.dword = *xl ;
    Xr.dword = *xr ;
    
    Xl.dword ^= PArray [17] ;
    ROUND (Xr, Xl, 16) ;  ROUND (Xl, Xr, 15) ;
    ROUND (Xr, Xl, 14) ;  ROUND (Xl, Xr, 13) ;
    ROUND (Xr, Xl, 12) ;  ROUND (Xl, Xr, 11) ;
    ROUND (Xr, Xl, 10) ;  ROUND (Xl, Xr, 9) ;
    ROUND (Xr, Xl, 8) ;   ROUND (Xl, Xr, 7) ;
    ROUND (Xr, Xl, 6) ;   ROUND (Xl, Xr, 5) ;
    ROUND (Xr, Xl, 4) ;   ROUND (Xl, Xr, 3) ;
    ROUND (Xr, Xl, 2) ;   ROUND (Xl, Xr, 1) ;
    Xr.dword ^= PArray[0];
    
    *xl = Xr.dword;
    *xr = Xl.dword;
}


// constructs the enctryption sieve
void Initialize (BYTE key[], int keybytes)
{
    int          i, j ;
    // グローバル変数にしたメンバの初期化
    //PArray = DWORD[18] ;
    //SBoxes = DWORD[4][256] ;
    PArray = (DWORD *)malloc(sizeof(DWORD) * (18 + 1));
    SBoxes = (DWORD **)malloc(sizeof(DWORD) * (4 + 1));
    for(i = 0; i < 4; i++){
        SBoxes[i] = (DWORD *)malloc(sizeof(DWORD) * (256 + 1));
    }
    // 変数宣言
    DWORD          data, datal, datar ;
    union aword temp ;
    
    // first fill arrays from data tables
    for (i = 0 ; i < 18 ; i++)
        PArray [i] = bf_P [i] ;
    
    for (i = 0 ; i < 4 ; i++)
    {
        for (j = 0 ; j < 256 ; j++)
            SBoxes [i][j] = bf_S [i][j] ;
    }
    
    
    j = 0 ;
    for (i = 0 ; i < NPASS + 2 ; ++i)
    {
        temp.dword = 0 ;
        temp.w.byte0 = key[j];
        temp.w.byte1 = key[(j+1) % keybytes] ;
        temp.w.byte2 = key[(j+2) % keybytes] ;
        temp.w.byte3 = key[(j+3) % keybytes] ;
        data = temp.dword ;
        PArray [i] ^= data ;
        j = (j + 4) % keybytes ;
    }
    
    datal = 0 ;
    datar = 0 ;
    
    for (i = 0 ; i < NPASS + 2 ; i += 2)
    {
        Blowfish_encipher (&datal, &datar) ;
        PArray [i] = datal ;
        PArray [i + 1] = datar ;
    }
    
    for (i = 0 ; i < 4 ; ++i)
    {
        for (j = 0 ; j < 256 ; j += 2)
        {
            Blowfish_encipher (&datal, &datar) ;
            SBoxes [i][j] = datal ;
            SBoxes [i][j + 1] = datar ;
        }
    }
}

// get output length, which must be even MOD 8
DWORD GetOutputLength (DWORD lInputLong)
{
    DWORD     lVal ;
    
    lVal = lInputLong % 8 ;    // find out if uneven number of bytes at the end
    if (lVal != 0)
        return lInputLong + 8 - lVal ;
    else
        return lInputLong ;
}

// Encode pIntput into pOutput.  Input length in lSize.  Returned value
// is length of output which will be even MOD 8 bytes.  Input buffer and
// output buffer can be the same, but be sure buffer length is even MOD 8.
DWORD Encode (BYTE * pInput, BYTE * pOutput, DWORD lSize)
{
    DWORD     lCount, lOutSize, lGoodBytes ;
    BYTE    *pi, *po ;
    int        i, j ;
    int        SameDest = (pInput == pOutput ? 1 : 0) ;
    
    lOutSize = GetOutputLength (lSize) ;
    for (lCount = 0 ; lCount < lOutSize ; lCount += 8)
    {
        if (SameDest)    // if encoded data is being written into input buffer
        {
            // kojima
            //            if (lCount < lSize - 7)    // if not dealing with uneven bytes at end
            if (lCount + 7 < lSize)    // if not dealing with uneven bytes at end
                //^^^^^
            {
                // kojima
                //                        Blowfish_encipher ((DWORD *) pInput,
                //                                           (DWORD *) (pInput + 4)) ;
                BlockEncrypt((DWORD *) pInput,
                             (DWORD *) (pInput + 4));
                //^^^^^
            }
            else        // pad end of data with null bytes to complete encryption
            {
                po = pInput + lSize ;    // point at bytepast the end of actual data
                j = (int) (lOutSize - lSize) ;    // number of bytes to set to null
                for (i = 0 ; i < j ; i++)
                    *po++ = 0 ;
                // kojima
                //                        Blowfish_encipher ((DWORD *) pInput,
                //                        (DWORD *) (pInput + 4)) ;
                BlockEncrypt((DWORD *) pInput,
                             (DWORD *) (pInput + 4));
                //^^^^^
            }
            pInput += 8 ;
        }
        else             // output buffer not equal to input buffer, so must copy
        {               // input to output buffer prior to encrypting
            //kojma
            //                        if (lCount < lSize - 7)    // if not dealing with uneven bytes at end
            if (lCount + 7 < lSize)
                //^^^^^
            {
                pi = pInput ;
                po = pOutput ;
                for (i = 0 ; i < 8 ; i++)
                    // copy bytes to output
                    *po++ = *pi++ ;
                
                // kojima
                //                            Blowfish_encipher ((DWORD *) pOutput,    // now encrypt them
                //                                    (DWORD *) (pOutput + 4)) ;
                BlockEncrypt((DWORD *) pOutput,    // now encrypt them
                             (DWORD *) (pOutput + 4)) ;
                //^^^^^
            }
            else        // pad end of data with null bytes to complete encryption
            {
                lGoodBytes = lSize - lCount ;    // number of remaining data bytes
                po = pOutput ;
                for (i = 0 ; i < (int) lGoodBytes ; i++)
                    *po++ = *pInput++ ;
                for (j = i ; j < 8 ; j++)
                    *po++ = 0 ;
                
                // kojima
                //                            Blowfish_encipher ((DWORD *) pOutput,
                //                                               (DWORD *) (pOutput + 4)) ;
                BlockEncrypt((DWORD *) pOutput,    // now encrypt them
                             (DWORD *) (pOutput + 4)) ;
                //^^^^^
            }
            pInput += 8 ;
            pOutput += 8 ;
        }
    }
    return lOutSize ;
}

// Decode pIntput into pOutput.  Input length in lSize.  Input buffer and
// output buffer can be the same, but be sure buffer length is even MOD 8.
void Decode (BYTE * pInput, BYTE * pOutput, DWORD lSize)
{
    DWORD     lCount ;
    BYTE    *pi, *po ;
    int        i ;
    int        SameDest = (pInput == pOutput ? 1 : 0) ;
    
    for (lCount = 0 ; lCount < lSize ; lCount += 8)
    {
        if (SameDest)    // if encoded data is being written into input buffer
        {
            // kojima
            //            Blowfish_decipher ((DWORD *) pInput,
            //                               (DWORD *) (pInput + 4)) ;
            BlockDecrypt((DWORD *) pInput,    // now encrypt them
                         (DWORD *) (pInput + 4)) ;
            //^^^^^
            pInput += 8 ;
        }
        else             // output buffer not equal to input buffer
        {               // so copy input to output before decoding
            pi = pInput ;
            po = pOutput ;
            for (i = 0 ; i < 8 ; i++)
                *po++ = *pi++ ;
            // kojima
            //            Blowfish_decipher ((DWORD *) pOutput,
            //                               (DWORD *) (pOutput + 4)) ;
            BlockDecrypt((DWORD *) pOutput,    // now encrypt them
                         (DWORD *) (pOutput + 4)) ;
            //^^^^^
            pInput += 8 ;
            pOutput += 8 ;
        }
    }
}

// kojima add function
void BlockEncrypt(DWORD *xl, DWORD *xr)
{
    *xl = htonl(*xl);
    *xr = htonl(*xr);
    
    Blowfish_encipher (xl, xr);
    
    *xl = ntohl(*xl);
    *xr = ntohl(*xr);
}
// kojima add function
void BlockDecrypt(DWORD *xl, DWORD *xr)
{
    *xl = htonl(*xl);
    *xr = htonl(*xr);
    
    Blowfish_decipher (xl, xr);
    
    *xl = ntohl(*xl);
    *xr = ntohl(*xr);
}
