//
//  QrViewController.swift
//  Q_ANIP
//
//  Created by L&A on 2017/09/25.
//  Copyright © 2017年 L&A. All rights reserved.
//

import UIKit
import CoreImage


class QrViewController: UIViewController,TcpDelegate,UITextFieldDelegate {
    
    @IBOutlet var scrollView:UIScrollView!
    @IBOutlet var contView:UIView!
    
    // 個人安否登録
    @IBOutlet var imgBlk01_01:UIImageView!
    @IBOutlet var lblBlk01_01:UILabel!

    // QR
    @IBOutlet var imgQrBack:UIImageView!
    @IBOutlet var lblBlk02_01:UILabel!
    @IBOutlet var imgQr:UIImageView!

    // 入力情報
    @IBOutlet var lblBlk03_01:UILabel!
    @IBOutlet var lblBlk03_01_01:UILabel!
    @IBOutlet var lblBlk03_02:UILabel!

    // 戻る
    @IBOutlet var btnBack:UIButton!
    // 送信
    @IBOutlet var btnSend:UIButton!
    private var mSendAlert: UIAlertController!
    private var mTcpCon = TcpConnection()
    
    // IP / Port TextFieldTag
    private var TAG_IP = 1
    private var TAG_PORT = 2
    // 前回入力情報キー
    private var KEY_IP = "IP"
    private var KEY_PORT = "Port"
    
    // 送信中ダイアログを表示しているか
    private var bDispSending = false
    // 送信中ダイアログを閉じるときの状態
    private var STATUS_SUCCESS = 0
    private var STATUS_QUEUE_FULL = 1
    private var STATUS_SERVER_FAULT = 2
    private var STATUS_FORMAT_ERROR = 3
    private var STATUS_DATA_ERROR = 4
    private var STATUS_COMPOSITE_FAILURE = 5
    private var STATUS_CONNECT_FAILURE = 6
    private var STATUS_TIMEOUT = 7
    private var STATUS_OTHER = -1
    
    // 結果ダイアログを表示しているか
    private var bDispAlert = false

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        // QR背景の角丸表示0
        imgQrBack.layer.cornerRadius = 30
        imgQrBack.layer.masksToBounds = true
        // ボタンにイベントを追加
        btnBack.addTarget(self, action: #selector(buttonEvent(sender:)), for: .touchUpInside)
        btnSend.addTarget(self, action: #selector(sendButtonEvent(sender:)), for: .touchUpInside)
        // ボタンの角丸表示0
        btnBack.layer.cornerRadius = 10
        btnBack.layer.masksToBounds = true
        btnSend.layer.cornerRadius = 10
        btnSend.layer.masksToBounds = true
        
        //  ナビゲーション 非表示
//        self.navigationController?.setNavigationBarHidden(true, animated: false)
        self.navigationItem.hidesBackButton = true
        
        // navigationHeightではなく、safeAreaInsets.Topを使用するよう変更
        //let navigationHeight = self.navigationController?.navigationBar.frame.height
        var viewTop : CGFloat = 0
        if #available(iOS 11.0, *){
            viewTop = (UIApplication.shared.keyWindow?.safeAreaInsets.top) ?? 0
        }
        
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size
        scrollView.frame = CGRect(x: 0, y: 0 + viewTop,
                                  width: scrnSize.width , height: 600.0)
        scrollView.contentSize = CGSize(width: scrnSize.width , height: 600.0)
        contView.frame = CGRect(x: 0, y: 0 + viewTop,
                                width: scrnSize.width , height: 600.0)

        // レイアウト情報
        var fYOffset:CGFloat = 0
        var fXOffset:CGFloat = 0
        var fWidth:CGFloat = 0
        
        //*************************************
        // 個人安否登録
        //*************************************
        var size = imgBlk01_01.bounds.size
        imgBlk01_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        fWidth = scrnSize.width - size.width
        size = lblBlk01_01.bounds.size
        lblBlk01_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += fWidth
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // QR
        //*************************************
        fXOffset = 26
        size = lblBlk02_01.bounds.size
        var yWorkOffset:CGFloat = 5
        fWidth = scrnSize.width - ( fXOffset * 2 )
        lblBlk02_01.frame = CGRect(x: fXOffset, y: fYOffset + yWorkOffset, width: fWidth, height: size.height)
        yWorkOffset += size.height + 5
        
        size = imgQr.bounds.size
        fXOffset = (scrnSize.width - size.width ) / 2
        imgQr.frame = CGRect(x: fXOffset, y: fYOffset + yWorkOffset, width: size.height, height: size.height)

        fXOffset = 16
        size = imgQrBack.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgQrBack.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 入力情報
        //*************************************
        fYOffset += 3
        fXOffset = 30
        size = lblBlk03_01.bounds.size
        lblBlk03_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)

        fYOffset += 1
        fXOffset = fXOffset + size.width + 10
        size = lblBlk03_01_01.bounds.size
        lblBlk03_01_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)

        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 48
        size = lblBlk03_02.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        lblBlk03_02.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // buttons
        //*************************************
        fXOffset = 16
        fWidth = scrnSize.width - ( fXOffset * 2 )
        
        // 戻る
        size = btnBack.bounds.size
        btnBack.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fYOffset += size.height + 5
        
        // 送信
        size = btnSend.bounds.size
        btnSend.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fYOffset += size.height + 5
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        
        print(ViewController.s_strCsvData as Any)
        //print(String(data: ViewController.s_strCsvData!, encoding: String.Encoding.shiftJIS))
        // QRコード生成のフィルター
        // NSData型でデーターを用意
        // inputCorrectionLevelは、誤り訂正レベル
        let qr = CIFilter(name: "CIQRCodeGenerator", parameters: ["inputMessage": ViewController.s_strCsvData as Any, "inputCorrectionLevel": "M"])!
        
        let sizeTransform = CGAffineTransform(scaleX: 10, y: 10)
        let qrImg = qr.outputImage!.transformed(by: sizeTransform)
        let uiQrImg = UIImage(ciImage: qrImg)
        imgQr.image = uiQrImg
        
        // 入力情報設定
        lblBlk03_02.text = ViewController.s_strDispItem
        lblBlk03_02.sizeToFit()

        // レイアウト情報
        var fYOffset:CGFloat = 0
        var fXOffset:CGFloat = 0
        var fWidth:CGFloat = 0

        let frame = lblBlk03_02.frame
        
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size
        
        fYOffset = frame.origin.y + frame.size.height + 5
        fXOffset = 16
        fWidth = scrnSize.width - ( fXOffset * 2 )
        // 戻るボタン
        var size = btnBack.bounds.size
        btnBack.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fYOffset += size.height + 5
        
        // 送信ボタン
        size = btnSend.bounds.size
        btnSend.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fYOffset += size.height + 10
        
        
        // スクリーンより小さいサイズを指定するとアニメーション時に入力画面が見えるため、
        // スクリーン以下の場合はスクリーンのサイズとする
        if fYOffset < scrnSize.height
        {
            fYOffset = scrnSize.height
        }
        // navigationHeightではなく、safeAreaInsets.Topを使用するよう変更
        //let navigationHeight = self.navigationController?.navigationBar.frame.height
        var viewTop : CGFloat = 0
        if #available(iOS 11.0, *){
            viewTop = (UIApplication.shared.keyWindow?.safeAreaInsets.top) ?? 0
        }

        scrollView.frame = CGRect(x: 0, y: 0 + viewTop,
                                  width: scrnSize.width , height: fYOffset)
        scrollView.contentSize = CGSize(width: scrnSize.width , height: fYOffset)
        contView.frame = CGRect(x: 0, y: 0 + viewTop,
                                width: scrnSize.width , height: fYOffset)

        // 時刻が設定されていなければ非表示
        if ViewController.s_strCsvTime.isEmpty
        {
            lblBlk03_01_01.isHidden = true
        }
        //　時刻が設定されていれば時刻を表示
        else
        {
            lblBlk03_01_01.isHidden = false
            lblBlk03_01_01.text = ViewController.s_strCsvTime
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        self.navigationController?.navigationBar.isUserInteractionEnabled = false
    }
    
    @objc func buttonEvent(sender: UIButton) {
        ChangeMainView()
    }
    @objc func sendButtonEvent(sender: UIButton){
// UDP受信処理 019-02-22 start
        // アプリケーションデリゲート取得
        let app: AppDelegate = UIApplication.shared.delegate as! AppDelegate
        if(!app.serverIP.isEmpty){
            // 前回入力情報へサーバIPとポート番号を上書き
            let ud = UserDefaults.standard
            ud.set(app.serverIP, forKey: self.KEY_IP)
            ud.set(app.serverPort, forKey: self.KEY_PORT)
        }
// UDP受信処理 019-02-22 end

// 送信DLGをはさまず、送信処理を開始する 019-03-01 start
//        // 送信DLGの表示
//        let alert: UIAlertController = UIAlertController(title: "安否登録情報送信", message: "", preferredStyle: .alert)
//
//        // buttons
//        let cancelAction: UIAlertAction = UIAlertAction(title: "キャンセル", style: .cancel, handler: {(action:UIAlertAction!) -> Void in
//
//        })
//        let doneAction: UIAlertAction = UIAlertAction(title: "送信", style: .default, handler: {(action:UIAlertAction!)-> Void in
//
//            // 入力を取得
//            var inputs : [String: String] = [:]
//            let textfields:Array<UITextField>? = alert.textFields as Array<UITextField>?
//            if( textfields != nil){
//                for tf:UITextField in textfields!{
//                    //ud.set(tf.text, forKey: tf.placeholder!)
//                    if(tf.text == ""){
//                        // 未入力がある
//                        break;
//                    }else{
//                        inputs[tf.placeholder!] = tf.text
//                    }
//                }
//            }
//
//            // 未入力の項目があり、countが2に満たない場合
//            if(inputs.count < 2){
//                // error ダイアログを表示
//                self.dispErrorAlert(title: "エラー", message: "正しく入力してください")
//            }
//            // 両方とも入力されている場合
//            else{
//                // 前回入力情報として保持する
//                let ud = UserDefaults.standard
//                for(key,value) in inputs {
//                    ud.set(value, forKey: key)
//                }
//
//                // 送信処理
//                self.showSendingAlert()
//                //self.SendData(strIp: ud.string(forKey: self.KEY_IP)!, strPort: ud.string(forKey: self.KEY_PORT)!)
//                self.SendData(strIp: inputs[self.KEY_IP]!, strPort: inputs[self.KEY_PORT]!)
//            }
//        })
//
//        alert.addAction(cancelAction)
//        alert.addAction(doneAction)
//
//        // textfields
//        let ud = UserDefaults.standard
//        let strIp = ud.string(forKey: KEY_IP)
//        let strPort = ud.string(forKey: KEY_PORT)
//        alert.addTextField(configurationHandler: {(text:UITextField!)-> Void in
//            text.placeholder = self.KEY_IP
//            text.delegate = self
//            if( strIp != nil){
//                text.text = strIp!
//            }
//            // 文字数制限
//            text.tag = self.TAG_IP
//            NotificationCenter.default.addObserver(
//                self,
//                selector: #selector(self.textFieldDidChange(notification:)),
//                name: UITextField.textDidChangeNotification,
//                object: text)
//            //
//
//        })
//        alert.addTextField(configurationHandler: {(text:UITextField!) -> Void in
//            text.placeholder = self.KEY_PORT
//            // 数字のみのキーボードにする
//            text.keyboardType = UIKeyboardType.numberPad
//            text.delegate = self
//            if( strPort != nil){
//                text.text = strPort!
//            }
//            // 文字数制限
//            text.tag = self.TAG_PORT
//            NotificationCenter.default.addObserver(
//                self,
//                selector: #selector(self.textFieldDidChange(notification:)),
//                name: UITextField.textDidChangeNotification,
//                object: text)
//
//
//        })
//
//        present(alert, animated: true, completion: nil)
        
        // 送信処理
        self.showSendingAlert()
        // IP,Port受信済なら送信処理を開始
        if(!app.serverIP.isEmpty && !app.serverPort.isEmpty)
        {
            self.SendData(strIp: app.serverIP, strPort: app.serverPort)
        }
        // IP,Portが空のときは送信失敗ダイアログを表示する
        else
        {
            closeSendingAlert(mode: STATUS_OTHER)
        }
        
// 送信DLGをはさまず、送信処理を開始する 019-03-01 end
        
    }
    
    @objc private func textFieldDidChange(notification: NSNotification) {
        let txtField = notification.object as! UITextField
        let textFieldString = txtField
        // IP
        if( txtField.tag == TAG_IP)
        {
            if let text = textFieldString.text {
                if text.count > 15 {
                    textFieldString.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:15)])
                }
            }
        }
        // Port
        else if( txtField.tag == TAG_PORT)
        {
            if let text = textFieldString.text {
                if text.count > 5 {
                    textFieldString.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:5)])
                }
            }
        }
    }
    
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString text: String) -> Bool {
        
        // 許可しない文字
        var str : String = ""
        if(textField.tag == TAG_IP){
            str = "^[0-9.]+$"
        }else if(textField.tag == TAG_PORT){
            str = "^[0-9]+$"
        }
        
        // 削除が押された場合は許可
        if text.isEmpty {
            return true
        }
            // 　削除以外の場合、数値以外が含まれるかどうかを判定する
        else {
            // 一文字ずつ判定する
            for charVal in text
            {
                // 数値以外の文字の場合は入力キャンセル
                let charTxt:String = String(charVal)
                //if charTxt.range(of: "^[0-9]+$", options: .regularExpression, range: nil, locale: nil) == nil {
                if charTxt.range(of: str, options: .regularExpression, range: nil, locale: nil) == nil {
                    return false
                }
            }
            // 全てが数値の時は入力受付
            return true
        }
    }
    
    func dispErrorAlert(title: String,message: String){
        
        // タイトル, メッセージ, Alertのスタイルを指定する
        // 第3引数のpreferredStyleでアラートの表示スタイルを指定する
        let alert: UIAlertController = UIAlertController(title: title, message: message, preferredStyle:  UIAlertController.Style.alert)
        
        // ② Actionの設定
        // Action初期化時にタイトル, スタイル, 押された時に実行されるハンドラを指定する
        // 第3引数のUIAlertActionStyleでボタンのスタイルを指定する
        // OKボタン
        let defaultAction: UIAlertAction = UIAlertAction(title: "OK", style: UIAlertAction.Style.default, handler:{
            // ボタンが押された時の処理を書く（クロージャ実装）
            (action: UIAlertAction!) -> Void in
        })
        
        // ③ UIAlertControllerにActionを追加
        alert.addAction(defaultAction)
        
        // ④ Alertを表示
        /* 送信中DLGが消しきれていないので、送信中DLGを親にDLGを表示する */
        if(mSendAlert != nil && !mSendAlert.isBeingDismissed){
            // 親ビューにするビューの検索
            var baseView = self.presentedViewController
            while((baseView!.presentedViewController) != nil){
                baseView = baseView!.presentedViewController;
            }
            
            baseView!.present(alert, animated: true, completion: nil)
        }
        /* 通常のAlert表示 */
        else{
            present(alert, animated: true, completion: nil)
        }
        
    }
    
    func ChangeMainView() {
        // Main画面 へ遷移する
        _ = self.navigationController?.popViewController(animated: true)
    }
    
    func SendData(strIp: String,strPort: String){
        
        // 電文フォーマット
        // ---------
        var msg = "";
        var msgByte = [UInt8]()
        
        var CsvDataMsg = "";
        
        // 個人安否情報
        //msg += String(data: ViewController.s_strCsvData!, encoding: String.Encoding.shiftJIS)!
        CsvDataMsg = String(data: ViewController.s_strCsvData!, encoding: String.Encoding.shiftJIS)!
        //CsvDataMsg = String(CsvDataMsg.suffix(CsvDataMsg.count - 1))
        
        let dataByte = [UInt8](CsvDataMsg.data(using: String.Encoding.shiftJIS)!)
        
        // データ部
//        msg = String(msg.count + (String)(msg.count).count) + msg
//        var count = msg.count
//        var lenByte = [UInt8](Data(bytes: &count, count: MemoryLayout.size(ofValue: count)))
        
        var count = (2 /*+ 1*/ + dataByte.count)
        var lenByte = [UInt8](Data(bytes: &count, count: MemoryLayout.size(ofValue: count)))
        
        // create Message
        // -----------
        // データ部
        msgByte.append(lenByte[0])
        msgByte.append(lenByte[1])
        // 暗号化フラグ
        //msgByte.append(encryptByte[0])
        //msgByte += [UInt8](Data(bytes: &encryptMsg, count: MemoryLayout.size(ofValue: encryptMsg)))
        
        // 個人安否情報
        msgByte += [UInt8](CsvDataMsg.data(using: String.Encoding.shiftJIS)!)
        
        msg = String(data: Data(bytes: msgByte), encoding: String.Encoding.shiftJIS)!
        print(msg)
        print(msgByte)
        
        // 送信
        // ---------
        DispatchQueue.global(qos: .default).async {
            self.mTcpCon.setIpPort(strIp: strIp, strPort: strPort)
            self.mTcpCon.delegate = self
            self.mTcpCon.connect()
            self.mTcpCon.send(sendData: msg)
            
            // Receive
            // -----
            // size 8以上でないと電文をStringに変換できない
            let res = self.mTcpCon.readData(size: 8)
            if(self.mTcpCon.getBConnect()){
                // 電文取得成功
                if( res.strRead != ""){
                    self.recv(strRes: res.strRead)
                }
                // 失敗(0byte受信)
                else{
                    self.closeSendingAlert(mode: self.STATUS_OTHER)
                }
            }else{
                self.closeSendingAlert(mode: self.STATUS_CONNECT_FAILURE)
            }
        }
        
    }
    func showSendingAlert(){
        mSendAlert = UIAlertController(title: "安否登録情報送信中…", message: "", preferredStyle: .alert)
        present(mSendAlert, animated: true, completion: nil)
        bDispSending = true
    }
    
    func recv(strRes: String){
        
        var resultMode = STATUS_OTHER
        
        var byteRes = [UInt8](strRes.data(using: .utf8)!)
        if(byteRes.count < 6){
            closeSendingAlert(mode: resultMode)
            return
        }
        // ResponceFormat
        //  データ部サイズ
        var byteDataLength = [UInt8]()
        byteDataLength.append(byteRes[1])
        byteDataLength.append(byteRes[0])
        //  応答コード
        var byteResCode = [UInt8]()
//        // シミュレータ
//        byteResCode.append(byteRes[3])
//        byteResCode.append(byteRes[2])
        // Windows
        byteResCode.append(byteRes[2])
        byteResCode.append(byteRes[3])
        //  詳細コード
        var byteDetailCode = [UInt8]()
//        // シミュレータ
//        byteDetailCode.append(byteRes[5])
//        byteDetailCode.append(byteRes[4])
        // Windows
        byteDetailCode.append(byteRes[4])
        byteDetailCode.append(byteRes[5])

        // byte -> Int変換
        let nResCode = Data(bytes: byteResCode)
                        .withUnsafeBytes({ (p: UnsafePointer<Int32>) in p.pointee })
        let nDetailCode = Data(bytes: byteDetailCode)
                        .withUnsafeBytes({ (p: UnsafePointer<Int32>) in p.pointee })
        
        // どちらも０（異常なし）の場合のみ、成功とする
        if( nResCode == 0 && nDetailCode == 0 ){
            resultMode = STATUS_SUCCESS
        }else{
            /* エラー分岐 */
            switch nDetailCode {
            case 1:
                resultMode = STATUS_QUEUE_FULL
                break;
            case 2:
                resultMode = STATUS_SERVER_FAULT
                break;
            case 3:
                resultMode = STATUS_FORMAT_ERROR
                break;
            case 4:
                resultMode = STATUS_DATA_ERROR
                break;
            case 5:
                resultMode = STATUS_COMPOSITE_FAILURE
                break;
            default:
                resultMode = STATUS_OTHER
                break;
            }
        }
        
        closeSendingAlert(mode: resultMode)
    }
    
    func DidRecv(tag: Int) {
        
    }
    // 接続失敗
    func DidConnectFailed() {
        closeSendingAlert(mode: STATUS_CONNECT_FAILURE)
    }
    
    func DidTimeOut() {
        closeSendingAlert(mode: STATUS_TIMEOUT)
    }
    // 切断
    func DidClose() {
        /* 現状は何もしない */
    }
    
    func closeSendingAlert(mode: Int){

        var strTitle = "エラー"
        var strMessage = ""
        
        switch mode {
        case STATUS_SUCCESS:
            /* 成功 */
            strTitle = "送信完了"
            strMessage = "送信完了しました。"
            break;
        case STATUS_QUEUE_FULL:
            strMessage = "しばらく時間をおいて再度送信してください。"
            break;
        case STATUS_SERVER_FAULT:
            strMessage = "サーバ側で障害が発生しました。\n送信に失敗しました。"
            break;
        case STATUS_FORMAT_ERROR:
            strMessage = "不正なフォーマットです。\n送信に失敗しました。"
            break;
        case STATUS_DATA_ERROR:
            strMessage = "不正なデータです。\n送信に失敗しました。"
            break;
        case STATUS_COMPOSITE_FAILURE:
            strMessage = "情報の復号に失敗したため避難所安否登録を失敗しました。"
            break;
        case STATUS_CONNECT_FAILURE:
            strMessage = "接続に失敗しました。"
            break;
        case STATUS_TIMEOUT:
            strMessage = "応答受信タイムアウトしました。\n送信に失敗しました。"
            break;
        case STATUS_OTHER:
            strMessage = "送信に失敗しました。"
            break;
        default:
            strMessage = "送信に失敗しました。"
            break;
        }
        
        // 送信中ダイアログ表示中で、(他のエラー/成功をタイムアウト表示で上書きするのを避けるため)
        // strMessageが空文字でないなら、ダイアログを表示
        if(bDispSending){
            // 送信中ダイアログを閉じる
            if(mSendAlert != nil){
                mSendAlert.dismiss(animated: false, completion: nil)
            }
            
            // ダイアログを表示
            dispErrorAlert(title: strTitle, message: strMessage)
            
            bDispSending = false
            mSendAlert = nil
            
            // TcpUtilsを閉じる
            if(mTcpCon.getBConnect()){
                mTcpCon.disconnect()
            }
        }
    }
}


