//
// Created by ysaijo on 2018/06/08.
// blowfish.cppを呼び出すためのcppファイル
//

#include <stdio.h>
#include <jni.h>
#include <string.h>
#include "blowfish.h"
#include "jniBlowfish.h"

#ifdef __cplusplus
extern "C" {
#endif

// Instance
static CBlowFish* getInstance(JNIEnv* env,jobject thiz){
    if(field.context == NULL){
        return NULL;
    }
    CBlowFish* const ptr = (CBlowFish*)env->GetLongField(thiz,field.context);
    return ptr;
}
static void setInstance(JNIEnv* env, jobject thiz,CBlowFish* bf){
    env->SetLongField(thiz,field.context,(jlong)bf);
}

// convert
static unsigned char* convertByteArrayToChar(JNIEnv *env,jbyteArray array){
    int len = env->GetArrayLength(array);

    unsigned char* buf = new unsigned char[len];

    env->GetByteArrayRegion(array,0,len, reinterpret_cast<jbyte*>(buf));

    return buf;
}
static jbyteArray convertCharToByteArray(JNIEnv *env, unsigned char* buf, int len){
    jbyteArray  array = env->NewByteArray(len);
    env->SetByteArrayRegion(array,0,len, reinterpret_cast<jbyte*>(buf));
    return array;
}
JNIEXPORT void JNICALL
//  Java_jp_co_nec_personinfoqrReader_CBlowfish_Instance(JNIEnv *env, jobject instance) {
    Java_jp_co_nec_personinfoqrGenelator_CBlowfish_Instance(JNIEnv *env, jobject instance) {

    // TODO
    jclass clazz;
    // クラス検索
    clazz = env->FindClass("jp/co/nec/personinfoqrGenelator/CBlowfish");
    if (clazz == NULL) {
        return;
    }

    field.context = env->GetFieldID(clazz, "mNativeContext", "J");
    if (field.context == NULL) {
        return;
    }

    CBlowFish *bf = new CBlowFish();
    setInstance(env, instance, bf);

}
JNIEXPORT void JNICALL
Java_jp_co_nec_personinfoqrGenelator_CBlowfish_Destroy(JNIEnv *env, jobject instance) {

    // TODO
    CBlowFish *ptr = getInstance(env, instance);
    if (ptr == NULL) {
        return;
    }

    if (ptr) {
        delete ptr;
    }
    ptr = NULL;
    field.context = NULL;
}

JNIEXPORT void JNICALL
Java_jp_co_nec_personinfoqrGenelator_CBlowfish_Initialize(JNIEnv *env, jobject instance,
                                                                    jbyteArray key_,
                                                                    jint keybytes) {
    //jbyte *key = env->GetByteArrayElements(key_, NULL);
    unsigned char *key = convertByteArrayToChar(env, key_);

    // TODO
    CBlowFish *bf = getInstance(env, instance);
    bf->Initialize(key, keybytes);

    key_ = convertCharToByteArray(env, key, sizeof(key));

}

JNIEXPORT jbyteArray JNICALL
Java_jp_co_nec_personinfoqrGenelator_CBlowfish_Encode(JNIEnv *env, jobject instance,
                                                                jbyteArray pInput_,
                                                                jbyteArray pOutput_, jlong lSize) {
    //jbyte *pInput = env->GetByteArrayElements(pInput_, NULL);
    //jbyte *pOutput = env->GetByteArrayElements(pOutput_, NULL);
    unsigned char *pInput = convertByteArrayToChar(env, pInput_);
    unsigned char *pOutput = convertByteArrayToChar(env, pOutput_);

    // TODO
    CBlowFish *bf = getInstance(env, instance);
    // kojima
//    bf->Encode(pInput, pOutput, lSize);
    DWORD lOutSize = bf->Encode(pInput, pOutput, lSize);
    //^^^^^

    //env->ReleaseByteArrayElements(pInput_, pInput, 0);
    //env->ReleaseByteArrayElements(pOutput_, pOutput, 0);
    pInput_ = convertCharToByteArray(env, pInput, sizeof(pInput));
    // kojima
//    pOutput_ = convertCharToByteArray(env, pOutput, sizeof(pOutput));
    pOutput_ = convertCharToByteArray(env, pOutput, lOutSize);
    //^^^^^

    return pOutput_;
}

JNIEXPORT jbyteArray JNICALL
Java_jp_co_nec_personinfoqrGenelator_CBlowfish_Decode(JNIEnv *env, jobject instance,
                                                                jbyteArray pInput_,
                                                                jbyteArray pOutput_, jlong lSize) {
    //jbyte *pInput = env->GetByteArrayElements(pInput_, NULL);
    //jbyte *pOutput = env->GetByteArrayElements(pOutput_, NULL);
    unsigned char *pInput = convertByteArrayToChar(env, pInput_);
    unsigned char *pOutput = convertByteArrayToChar(env, pOutput_);

    // TODO
    CBlowFish *bf = getInstance(env, instance);
    bf->Decode(pInput, pOutput, lSize);

    //env->ReleaseByteArrayElements(pInput_, pInput, 0);
    //env->ReleaseByteArrayElements(pOutput_, pOutput, 0);
    pInput_ = convertCharToByteArray(env, pInput, sizeof(pInput));

    // omiya
    jlong decodeSize = (jlong)strlen((char*)pOutput);
    //    unsigned char sOut[n];
    //    strcpy((char*)sOut, (char*)pOutput);
    //^^^^^

    //kojima
    //    pOutput_ = convertCharToByteArray(env, pOutput, sizeof(pOutput));
    //pOutput_ = convertCharToByteArray(env, pOutput, lSize);
    pOutput_ = convertCharToByteArray(env, pOutput, decodeSize);
    //^^^^^

    return pOutput_;
}

#ifdef __cplusplus
};
#endif

