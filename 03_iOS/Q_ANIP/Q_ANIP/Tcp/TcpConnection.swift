//
//  TcpUtils.swift
//  Q_ANIP
//
//  Created by user1 on 2018/08/02.
//  Copyright © 2018年 L&A. All rights reserved.
//

import Foundation

@objc protocol TcpDelegate{
    @objc optional func DidRecv()
    @objc optional func DidClose()
    @objc optional func DidConnectFailed()
    @objc optional func DidTimeOut()
}

class TcpConnection : NSObject,StreamDelegate {
    
    var delegate: TcpDelegate?
    
    private var serverIp: CFString!
    private var serverPort: UInt32!
    
    private var inputStream: InputStream!
    private var outputStream: OutputStream!
    
    private var bConnect = false;
    public func getBConnect() -> Bool { return bConnect }

    private var timer : Timer!
    private var TIMER_INTERVAL = 10
    
    private var TIMER_ID_CONNECT = 0
    private var TIMER_ID_RECEIVE = 1
    
    private var NOW_TIMER_MODE = -1
    
    public func setIpPort(strIp: String,strPort: String){
        serverIp = NSString(string: strIp)
        if(Int(strPort) != nil){
            serverPort = UInt32(Int(strPort)!)
        }
    }
    
    func connect(){
        
        DispatchQueue.global(qos: .default).async {
            self.setTimer(timerID: self.TIMER_ID_CONNECT)
        }
        
        var readStream: Unmanaged<CFReadStream>?
        var writeStream: Unmanaged<CFWriteStream>?
        
        CFStreamCreatePairWithSocketToHost(nil, self.serverIp, self.serverPort, &readStream, &writeStream)
        
        self.inputStream = readStream!.takeRetainedValue()
        self.outputStream = writeStream!.takeRetainedValue()
        
        self.inputStream.delegate = self
        self.outputStream.delegate = self
        
        self.inputStream.schedule(in: .current, forMode: RunLoop.Mode.default)
        self.outputStream.schedule(in: .current, forMode: RunLoop.Mode.default)
        
        self.inputStream.open()
        self.outputStream.open()
        
        bConnect = true
    }
    
    func stream(_ aStream: Stream, handle eventCode: Stream.Event) {
        if( aStream == self.inputStream){
            switch eventCode{
            case Stream.Event.errorOccurred:
                /* 何らかのエラーが発生 */
                print(aStream)
                print((aStream.streamError?.localizedDescription)!)
                /* スレッドの違いにより、ダイアログ表示に支障をきたすのでコメントアウト */
                //self.delegate?.DidConnectFailed!()
                bConnect = false
                break;
            case Stream.Event.hasBytesAvailable:
                read()
                break;
            case Stream.Event.endEncountered:
                /* サーバーからの切断 */
                self.delegate?.DidClose!()
                break;
            default:
                break;
            }
        }else{
            switch eventCode{
            case Stream.Event.errorOccurred:
                /* 何らかのエラーが発生 */
                print(aStream)
                print((aStream.streamError?.localizedDescription)!)
                /* スレッドの違いにより、ダイアログ表示に支障をきたすのでコメントアウト */
                //self.delegate?.DidConnectFailed!()
                bConnect = false
                break;
            case Stream.Event.endEncountered:
                /* 切断 */
                disconnect()
                break;
            default:
                break;
            }
        }
    }
    // タイムアウト
    public func setTimer(timerID: Int){
        
        // Timerは交互に動作する
        if(NOW_TIMER_MODE == timerID && bConnect){
            return
        }
        
        // 前のTimerをinvalidateしてからでないと起動させない
        if(timer != nil){
            stopTimer()
        }
        
        NOW_TIMER_MODE = timerID
        timer = Timer.scheduledTimer(timeInterval: TimeInterval(TIMER_INTERVAL),
                                     target: self,
                                     selector: #selector(self.TimeUp),
                                     userInfo: nil,
                                     repeats: false)
        RunLoop.current.add(timer, forMode: RunLoop.Mode.default)
        RunLoop.current.run()

    }
    @objc func TimeUp(){
        if(NOW_TIMER_MODE == TIMER_ID_CONNECT){
            self.delegate?.DidConnectFailed!()
        }else if(NOW_TIMER_MODE == TIMER_ID_RECEIVE){
            self.delegate?.DidTimeOut!()
        }
    }
    
    // タイマーストップ
    public func stopTimer(){
        if(timer != nil){
            timer.invalidate()
        }
    }
    
    // 受信
    func read() {
        self.delegate?.DidRecv!()
    }
    
    func readData(size: Int) -> (size: Int, strRead: String){
        
        var bufferSize = size
        var readSize = 0
        var strRead = ""
        
        // 接続失敗
        if(!bConnect){
            return (readSize,strRead)
        }
        
        // 応答タイマースタート
        DispatchQueue.global(qos: .default).async {
            self.stopTimer()
            self.setTimer(timerID: self.TIMER_ID_RECEIVE)
        }
        
        while(!inputStream.hasBytesAvailable){
            bufferSize = size - readSize
            var buffer = Array<UInt8>(repeating: 0, count: bufferSize)
            let bytesRead = inputStream.read(&buffer, maxLength: bufferSize)
            
            if( bytesRead > 0){
                let read = String(bytes: buffer, encoding: String.Encoding.shiftJIS)!
                strRead += read
                readSize += bytesRead
                
                // lengthは一律なのでbreakでも支障なし？
                break;
                
//                if(readSize >= size){
//                    break
//                }
            }else{
                // 0バイト受信 = 切断
                self.delegate?.DidClose!()
                strRead = ""
                break
            }
        }
        
        return (readSize,strRead)
    }
    
    //送信
    func send(sendData: String){
        //let data: NSData = sendData.data(using: String.Encoding.utf8)! as NSData
        let data: NSData = sendData.data(using: .shiftJIS)! as NSData
        var buffer = [UInt8](repeating: 0, count: data.length)
        data.getBytes(&buffer, length: data.length * MemoryLayout<UInt8>.size)
        print(buffer)
        
        self.outputStream.write(buffer, maxLength: data.length * MemoryLayout<UInt8>.size)
    }
    
    func disconnect(){
        self.inputStream.delegate = nil
        self.outputStream.delegate = nil
        
        self.inputStream.close()
        self.outputStream.close()
        
        self.inputStream.remove(from: .current, forMode: RunLoop.Mode.default)
        self.outputStream.remove(from: .current, forMode: RunLoop.Mode.default)
        
        bConnect = false
        
        stopTimer()
        NOW_TIMER_MODE = -1
    }
}
