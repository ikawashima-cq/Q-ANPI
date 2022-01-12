//
//  AppDelegate.swift
//  Q_ANIP
//
//  Created by L&A on 2017/09/25.
//  Copyright © 2017年 L&A. All rights reserved.
//

import UIKit
import SwiftSocket      // UDP受信処理 019-02-22

@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate {

    var window: UIWindow?

// UDP受信処理 019-02-22 start
    // サーバIPアドレス
    var serverIP: String = ""
    // サーバ通信ポート番号
    var serverPort: String = ""

    // UDP通信クラス
    private var udpSocket: UDPServer? = nil
    // UDP受信ポート
    private let UDP_PORT: Int32 = 56412
    // UDP受信スレッド終了フラグ
    private var isStopReceive: Bool = false
// UDP受信処理 019-02-22 end
    
    func application(_ application: UIApplication, didFinishLaunchingWithOptions launchOptions: [UIApplication.LaunchOptionsKey: Any]?) -> Bool {
        // Override point for customization after application launch.
// UDP受信処理 019-02-22 start
        // UDP受信開始
        startReceive()
// UDP受信処理 019-02-22 end
        return true
    }

    func applicationWillResignActive(_ application: UIApplication) {
        // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
        // Use this method to pause ongoing tasks, disable timers, and invalidate graphics rendering callbacks. Games should use this method to pause the game.
    }

    func applicationDidEnterBackground(_ application: UIApplication) {
        // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
        // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
    }

    func applicationWillEnterForeground(_ application: UIApplication) {
        // Called as part of the transition from the background to the active state; here you can undo many of the changes made on entering the background.
    }

    func applicationDidBecomeActive(_ application: UIApplication) {
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
    }

    func applicationWillTerminate(_ application: UIApplication) {
        // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
// UDP受信処理 019-02-22 start
        // UDP受信停止
        stopReceive()
// UDP受信処理 019-02-22 end
    }

// UDP受信処理 019-02-22 start
    // UDP受信開始
    private func startReceive() {
        // UDP接続
        self.udpSocket = UDPServer(address: "", port: UDP_PORT)
        if self.udpSocket != nil {
            // UDP受信スレッド起動
            receiveThread()
        }
    }
    
    // UDP受信停止
    private func stopReceive() {
        // UDP受信スレッド終了
        self.isStopReceive = true
        // 待機してから解放
        DispatchQueue.main.asyncAfter(deadline: .now() + 0.5) {
            // UDP切断
            if let socket = self.udpSocket {
                socket.close()
                self.udpSocket = nil
            }
        }
    }

    // UDP受信スレッド
    private func receiveThread() {
        // UDP受信スレッド開始
        self.isStopReceive = false
        
        // UDP受信スレッド起動
        DispatchQueue.global(qos: .default).async {
            // UDP受信スレッド終了まで繰り返す
            while(!self.isStopReceive) {
                if let socket = self.udpSocket {
                    // UDP受信
                    let(byteArray, senderIP, _) = socket.recv(100)
                    // UDP受信データを文字列へ変換
                    if let byteArray = byteArray, let string = String(data: Data(byteArray), encoding: .utf8) {
                        // 識別文字の"ANPI_"が含まれる場合
                        if string.contains("ANPI_") {
                            // 受信文字列を"_"で分割
                            let strArray = string.components(separatedBy: "_")
                            if strArray.count >= 2 {
                                // 送信元IPアドレスとポート番号設定
                                self.serverIP = senderIP
                                self.serverPort = strArray[1]
                            }
                        }
                    }
                }
            }
        }
    }
// UDP受信処理 019-02-22 end
}

