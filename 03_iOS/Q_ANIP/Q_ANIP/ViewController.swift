//
//  ViewController.swift
//  Q_ANIP
//
//  Created by L&A on 2017/09/25.
//  Copyright © 2017年 L&A. All rights reserved.
//

import UIKit

class ViewController: UIViewController, UITableViewDelegate, UITableViewDataSource, UITextFieldDelegate, UITextViewDelegate {

    @IBOutlet var scrollView:UIScrollView!
    @IBOutlet var contView:UIView!
    
    // 個人安否登録
    @IBOutlet var imgBlk01_01:UIImageView!
    @IBOutlet var lblBlk01_01:UILabel!
    @IBOutlet var btnBlk01_01:UIButton!
//    @IBOutlet var lblBlk01_02:UILabel!
    @IBOutlet var btnBlk01_02:UIButton!
    @IBOutlet var imgBlk01_02:UIImageView!

    // 電話番号
    @IBOutlet var lblBlk02_01:UILabel!
    @IBOutlet var lblBlk02_02:UILabel!
    @IBOutlet var lblBlk02_03:UILabel!
    @IBOutlet var lblBlk02_04:UILabel!
    @IBOutlet var lblBlk02_05:UILabel!
    @IBOutlet var txtPhone:UITextField!
    @IBOutlet var imgBlk02_01:UIImageView!

    // 名前　-> 姓
    @IBOutlet var lblBlk03_01:UILabel!
    @IBOutlet var lblBlk03_02:UILabel!
    @IBOutlet var lblBlk03_03:UILabel!
    @IBOutlet var lblBlk03_04:UILabel!
    @IBOutlet var lblBlk03_05:UILabel!
    @IBOutlet var txvName:UITextView!
    @IBOutlet var imgBlk03_01:UIImageView!
    // 名
    @IBOutlet var lblBlk14_01:UILabel!
    @IBOutlet var lblBlk14_02:UILabel!
    @IBOutlet var lblBlk14_03:UILabel!
    @IBOutlet var lblBlk14_04:UILabel!
    @IBOutlet var txvGivenname:UITextView!
    @IBOutlet var imgBlk14_01:UIImageView!
    
    // 年齢
    @IBOutlet var lblBlk04_01:UILabel!
    @IBOutlet var lblBlk04_02:UILabel!
    @IBOutlet var lblBlk04_03:UILabel!
    @IBOutlet var lblBlk04_04:UILabel!
    @IBOutlet var lblBlk04_05:UILabel!
    @IBOutlet var lblBlk04_06: UILabel!
    @IBOutlet var lblBlk04_07: UILabel!
    @IBOutlet var lblBlk04_08: UILabel!
    @IBOutlet var lblBlk04_09: UILabel!
    //@IBOutlet var txtAge:UITextField!
    @IBOutlet var txtYear: UITextField!
    @IBOutlet var txtMonth: UITextField!
    @IBOutlet var txtDate: UITextField!
    @IBOutlet var imgBlk04_01:UIImageView!
    private var datePicker: UIDatePicker = UIDatePicker()

    // 性別
    @IBOutlet var lblBlk05_01:UILabel!
    @IBOutlet var lblBlk05_02:UILabel!
    @IBOutlet var lblBlk05_03:UILabel!
    @IBOutlet var lblBlk05_04:UILabel!
    @IBOutlet var tblSexSel:UITableView!
    @IBOutlet var imgBlk05_01:UIImageView!

    // 入所・退所・在宅
    @IBOutlet var lblBlk06_01:UILabel!
    @IBOutlet var lblBlk06_02:UILabel!
    @IBOutlet var tblShelterSel:UITableView!
    @IBOutlet var imgBlk06_01:UIImageView!

    // 公表
    @IBOutlet var lblBlk07_01:UILabel!
    @IBOutlet var lblBlk07_02:UILabel!
    @IBOutlet var lblBlk07_03:UILabel!
    @IBOutlet var tblReleaseSel:UITableView!
    @IBOutlet var imgBlk07_01:UIImageView!

    // 任意項目表示
    @IBOutlet var lblDisp_01:UILabel!
    @IBOutlet var btnDisp_01:UIButton!
    @IBOutlet var imgDisp_01:UIImageView!

    // 住所
    @IBOutlet var lblBlk08_01:UILabel!
    @IBOutlet var txvAddress:UITextView!
    @IBOutlet var imgBlk08_01:UIImageView!

    // 怪我
    @IBOutlet var lblBlk09_01:UILabel!
    @IBOutlet var tblInjurySel:UITableView!
    @IBOutlet var imgBlk09_01:UIImageView!

    // 介護
    @IBOutlet var lblBlk10_01:UILabel!
    @IBOutlet var tblCareSel:UITableView!
    @IBOutlet var imgBlk10_01:UIImageView!

    // 障がい
    @IBOutlet var lblBlk11_01:UILabel!
    @IBOutlet var tblFailureSel:UITableView!
    @IBOutlet var imgBlk11_01:UIImageView!

    // 妊産婦
    @IBOutlet var lblBlk12_01:UILabel!
    @IBOutlet var tblPregnantSel:UITableView!
    @IBOutlet var imgBlk12_01:UIImageView!
    
    // 暗号化チェック
    @IBOutlet var btnEncrypt: UIButton!
    @IBOutlet var imgBlk13_01: UIImageView!
    private var bEncryptFlag : Bool = false
    
    @IBOutlet var btnConfirmation:UIButton!
    @IBOutlet var btnPrivacyPolicy:UIButton!
    
    public static var s_strCsv:String = ""
    public static var s_strDispItem:String = ""
    public static var s_strCsvData:Data? = nil
    public static var s_bEncryptFlg:Bool = false
    public static var s_strCsvTime:String = ""

    private var bFirstSelect = false
    
//    var statusBarHidden = false
//    override var prefersStatusBarHidden: Bool {
//        return statusBarHidden
//    }

    // 表形式
    let dataSex = ["男性:Male","女性:Female"]
    let dataShelter = ["避難所への入所","避難所からの退所","在宅避難\n（支援は受けるが自宅で\n                            寝泊まりする）"]
    let dataRelease = ["許可","拒否"]
    let dataInjury = ["無し","有り"]
    let dataCare = ["不要","必要"]
    let dataFailure = ["無し","有り"]
    let dataPregnant = ["いいえ","はい"]
    
    // QR画面表示用
    let qrDataSex = ["男性","女性"]
    let qrDataShelter = ["入所","退所","在宅"]
    //let qrDataRelease = ["許可","拒否"]
    let qrDataRelease = ["拒否","許可"]     // 2019.04.03 Modify: Bug Fix 0:拒否、1:許可
    let qrDataInjury = ["無し","有り"]
    let qrDataCare = ["不要","必要"]
    let qrDataFailure = ["無し","有り"]
    let qrDataPregnant = ["いいえ","はい"]
    let qrTitle = ["電話番号:", "氏名:", "生年月日:", "性別:", "入所状況:", "公表可否:", "住所:", "怪我の有無:", "要介護:", "障がいの有無:", "妊産婦:"]
    
    // キー名称定義
    static let KEY_PRE_DATA = "PreData"
    
    let URL_PRIVACY_POLICY: String = "http://qzss.go.jp/technical/q-anpi/privacy-policy.html"

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        tblSexSel.dataSource = self
        tblSexSel.delegate = self
        // テーブルの角丸表示
        tblSexSel.layer.cornerRadius = 10
        tblSexSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblSexSel.allowsMultipleSelection = false
        tblSexSel.tableFooterView = UIView(frame: .zero)
        
        tblShelterSel.dataSource = self
        tblShelterSel.delegate = self
        // テーブルの角丸表示
        tblShelterSel.layer.cornerRadius = 10
        tblShelterSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblShelterSel.allowsMultipleSelection = false
        tblShelterSel.tableFooterView = UIView(frame: .zero)
        
        tblReleaseSel.dataSource = self
        tblReleaseSel.delegate = self
        // テーブルの角丸表示
        tblReleaseSel.layer.cornerRadius = 10
        tblReleaseSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblReleaseSel.allowsMultipleSelection = false
        tblReleaseSel.tableFooterView = UIView(frame: .zero)
        
        tblInjurySel.dataSource = self
        tblInjurySel.delegate = self
        // テーブルの角丸表示
        tblInjurySel.layer.cornerRadius = 10
        tblInjurySel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblInjurySel.allowsMultipleSelection = false
        tblInjurySel.tableFooterView = UIView(frame: .zero)
        
        tblCareSel.dataSource = self
        tblCareSel.delegate = self
        // テーブルの角丸表示
        tblCareSel.layer.cornerRadius = 10
        tblCareSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblCareSel.allowsMultipleSelection = false
        tblCareSel.tableFooterView = UIView(frame: .zero)
        
        tblFailureSel.dataSource = self
        tblFailureSel.delegate = self
        // テーブルの角丸表示
        tblFailureSel.layer.cornerRadius = 10
        tblFailureSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblFailureSel.allowsMultipleSelection = false
        tblFailureSel.tableFooterView = UIView(frame: .zero)
        
        tblPregnantSel.dataSource = self
        tblPregnantSel.delegate = self
        // テーブルの角丸表示
        tblPregnantSel.layer.cornerRadius = 10
        tblPregnantSel.layer.masksToBounds = true
        // trueで複数選択、falseで単一選択
        tblPregnantSel.allowsMultipleSelection = false
        tblPregnantSel.tableFooterView = UIView(frame: .zero)

        // テキストビューの枠線表示
        txvName.layer.borderColor = UIColor.init(red: 0.9, green: 0.9, blue: 0.9, alpha: 1.0).cgColor
        txvName.layer.borderWidth = 1.0
        // テキストビューの角丸表示
        txvName.layer.cornerRadius = 5.0
        txvName.layer.masksToBounds = true
        
        // テキストビューの枠線表示
        txvGivenname.layer.borderColor = UIColor.init(red: 0.9, green: 0.9, blue: 0.9, alpha: 1.0).cgColor
        txvGivenname.layer.borderWidth = 1.0
        // テキストビューの角丸表示
        txvGivenname.layer.cornerRadius = 5.0
        txvGivenname.layer.masksToBounds = true

        // テキストビューの枠線表示
        txvAddress.layer.borderColor = UIColor.init(red: 0.9, green: 0.9, blue: 0.9, alpha: 1.0).cgColor
        txvAddress.layer.borderWidth = 1.0
        // テキストビューの角丸表示
        txvAddress.layer.cornerRadius = 5.0
        txvAddress.layer.masksToBounds = true
        
        // 完了時にキーボードをしまうための設定
        txtPhone.delegate = self //
        //txtAge.delegate = self //
        NotificationCenter.default.addObserver(
            self,
            selector: #selector(textFieldDidChange),
            name: UITextField.textDidChangeNotification,
            object: txtPhone)
        //        NotificationCenter.default.addObserver(
        //            self,
        //            selector: #selector(textFieldDidChange),
        //            name: NSNotification.Name.UITextFieldTextDidChange,
        //            object: txtAge)
        
        // DatePicker
        txtYear.delegate = self
        txtMonth.delegate = self
        txtDate.delegate = self
        
        datePicker.datePickerMode = .date
        datePicker.addTarget(self, action: #selector(self.changedDate(sender:)), for: UIControl.Event.valueChanged)
        // 西暦表示固定
        datePicker.calendar = Calendar(identifier: .gregorian)
        // 現在から120年前までを範囲とする
        let calendar = Calendar.current
        let minYear = calendar.date(byAdding: .year, value: -120, to: Date())
        datePicker.minimumDate = minYear!
        datePicker.maximumDate = Date()
        
        // TextFieldとdatePickerを紐付け
        txtYear.inputView = datePicker
        txtMonth.inputView = datePicker
        txtDate.inputView = datePicker
        // ツールバー
        let pickerToolbar = UIToolbar()
        pickerToolbar.barStyle = .default
        pickerToolbar.isTranslucent = true
        pickerToolbar.tintColor = UIColor.black
        let doneButton = UIBarButtonItem(title: "Done", style: .done, target: self, action: #selector(self.onClick_done))
        let spaceButton = UIBarButtonItem(barButtonSystemItem: .flexibleSpace, target: self, action: nil)
        let cancelButton = UIBarButtonItem(title: "Cancel", style: .plain, target: self, action: #selector(self.onClick_cancel))
        pickerToolbar.setItems([cancelButton,spaceButton,doneButton], animated: false)
        pickerToolbar.isUserInteractionEnabled = true
        pickerToolbar.sizeToFit()
        txtYear.inputAccessoryView = pickerToolbar
        txtMonth.inputAccessoryView = pickerToolbar
        txtDate.inputAccessoryView = pickerToolbar
        
        // 完了時にキーボードをしまうための設定
        txvName.delegate = self //
        txvGivenname.delegate = self
        txvAddress.delegate = self //
        
        NotificationCenter.default.addObserver(
            self,
            selector: #selector(textViewDidChange),
            name: UITextView.textDidChangeNotification,
            object: txvName)
        NotificationCenter.default.addObserver(
            self,
            selector:  #selector(textViewDidChange),
            name: UITextView.textDidChangeNotification,
            object: txvGivenname)
        NotificationCenter.default.addObserver(
            self,
            selector: #selector(textViewDidChange),
            name: UITextView.textDidChangeNotification,
            object: txvAddress)

        // ボタンにイベントを追加
        btnConfirmation.addTarget(self, action: #selector(buttonEvent(sender:)), for: .touchUpInside)
        // ボタンにイベントを追加
        btnDisp_01.addTarget(self, action: #selector(dispButtonEvent(sender:)), for: .touchUpInside)
        // ボタンにイベントを追加
        btnBlk01_01.addTarget(self, action: #selector(clearButtonEvent(sender:)), for: .touchUpInside)
        // ボタンにイベントを追加
        btnBlk01_02.addTarget(self, action: #selector(preButtonEvent(sender:)), for: .touchUpInside)
        // ボタンにイベントを追加
        btnEncrypt.addTarget(self, action: #selector(checkButtonEvent(sender:)), for: .touchUpInside)
        // ボタンにイベントを追加
        btnPrivacyPolicy.addTarget(self, action: #selector(privacyPolicyButtonEvent(sender:)), for: .touchUpInside)
        
        //  ナビゲーション 非表示
        //self.navigationController?.setNavigationBarHidden(true, animated: false)
        // navigationHeightではなく、safeAreaInsets.Topを使用するよう変更
        let navigationHeight = self.navigationController?.navigationBar.frame.height
        var viewTop : CGFloat = 0
        if #available(iOS 11.0, *){
            viewTop = (UIApplication.shared.keyWindow?.safeAreaInsets.top) ?? 0
        }
        
        if viewTop < navigationHeight! {
            viewTop = navigationHeight!
        }
        
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size
//        scrollView.frame = CGRect(x: 0, y: 0 - viewTop, width: scrnSize.width , height: 1742)
//        contView.frame = CGRect(x: 0, y: 0 - viewTop, width: scrnSize.width , height: 1742)
//        scrollView.frame = CGRect(x: 0, y: 0, width: scrnSize.width , height: 1742)
//        contView.frame = CGRect(x: 0, y: 0, width: scrnSize.width , height: 1742)
        scrollView.frame = CGRect(x: 0, y: 0 - viewTop, width: scrnSize.width , height: 1950)
        contView.frame = CGRect(x: 0, y: 0 - viewTop, width: scrnSize.width , height: 1950)

        // レイアウト情報
        DispNonDetailItem()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.navigationController?.navigationBar.sizeToFit()
    }
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        
        DispatchQueue.main.async {
            self.navigationController?.navigationBar.isUserInteractionEnabled = false
        }
        
        // ここより前に呼び出すと選択項目の下の区切り線が消えてしまうため、ここで呼び出す
        // 毎回呼び出すと選択状態が変わるため、一度のみ呼び出す
        // 入所・退所・在宅のみ１行目をデフォルトで選択
        if !bFirstSelect {
            let indexPath = IndexPath(row: 0, section: 0)
            tblShelterSel.selectRow(at: indexPath, animated: false, scrollPosition: UITableView.ScrollPosition.none)
            CheckTableCell(tableView: tblShelterSel, indexPath: indexPath, check:true)
            
            bFirstSelect = true
        }

        // 前回データが存在していれば前回登録情報のボタンを有効とする
        let ud = UserDefaults.standard
        let strPreVal = ud.string(forKey: ViewController.KEY_PRE_DATA)
        btnBlk01_02.isEnabled = false
        btnBlk01_02.backgroundColor = UIColor.gray
        if strPreVal != nil
        {
            if !(strPreVal?.isEmpty)!
            {
                btnBlk01_02.isEnabled = true
                btnBlk01_02.backgroundColor = UIColor.init(red: 0, green: 0.5, blue: 1, alpha: 1)
            }
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    

    @objc func buttonEvent(sender: UIButton) {
        //*************************************
        //　入力内容の取得
        //*************************************
        // 電話番号
        let strPhone = txtPhone.text
        // 名前
        let strFamilyname = txvName.text            // 2019.04.03 Add: Bug Fix
        let strGivenname = txvGivenname.text        // 2019.04.03 Add: Bug Fix
        let strName = txvName.text + "　" + txvGivenname.text
        let strDispName = txvName.text + " " + txvGivenname.text
        // 年齢
        //let strAge = txtAge.text
        // 生年月日 - 年
        let strYear = txtYear.text
        //           月
        let strMonth = txtMonth.text
        //           日
        let strDate = txtDate.text
        
        let strDispBirth = strYear! + "-" + strMonth! + "-" + strDate!
        let strBirth = strYear! + strMonth! + strDate!
        // 性別
        let nSex = tblSexSel.indexPathForSelectedRow
        // 入所・退所・在宅
        let nShelter = tblShelterSel.indexPathForSelectedRow
        // 公表
        let nRelease = tblReleaseSel.indexPathForSelectedRow
        // 住所
        let strAddress = txvAddress.text
        // 怪我
        let nInjury = tblInjurySel.indexPathForSelectedRow
        // 介護
        let nCare = tblCareSel.indexPathForSelectedRow
        // 障がい
        let nFailure = tblFailureSel.indexPathForSelectedRow
        // 妊産婦
        let nPregnant = tblPregnantSel.indexPathForSelectedRow
        // 暗号化フラグ
        bEncryptFlag = btnEncrypt.isSelected
        var bEncrypt = "0"
        if( bEncryptFlag ){
            bEncrypt = "1"
        }else{
            bEncrypt = "0"
        }
        
        //*************************************
        //　入力内容のチェック
        //*************************************
        var bErr = false
        var strErrMsg = ""
        // 電話番号
        if( (strPhone?.count)! < 1 )
        {
            strErrMsg += "電話番号を入力してください。\n"
            bErr = true
        }
        if( (strPhone?.count)! > 12 )
        {
            strErrMsg += "電話番号は12桁までです。\n"
            bErr = true
        }
        // 名前
        //if( strName.count < (1 + 1) ) // スペース分追加
        if( (strFamilyname?.count)! < 1 || (strGivenname?.count)! < 1)      // 2019.04.03 Modify: Bug Fix 性、名どちらかでも未入力でエラー
        {
            strErrMsg += "名前を入力してください。\n"
            bErr = true
        }
        if( strName.count > 25 ) // 12 + 12 + 1(全角スペース)なのでスペース分増加なし
        {
            strErrMsg += "名前は２５文字までです。\n"
            bErr = true
        }
        if( !(strName.canBeConverted(to: String.Encoding.shiftJIS)) )
        {
            strErrMsg += "名前に変換できない文字が含まれています。\n"
            bErr = true
        }
        // 年齢
//        if( (strAge?.count)! < 1 )
//        {
//            strErrMsg += "年齢を入力してください。\n"
//            bErr = true
//        }
//        if( (strAge?.count)! > 3 )
//        {
//            strErrMsg += "年齢は３桁までです。\n"
//            bErr = true
//        }
        // 生年月日・未入力
        var bBDErr = false
        let df = DateFormatter()
        df.dateFormat = "yyyy-MM-dd"
        df.timeZone = TimeZone.current
        df.locale = Locale(identifier: "en_US_POSIX")
        if( (strDispBirth.count) < ( 1 + 2) ){  // [ - ]分増加
            strErrMsg += "生年月日を正しく入力してください。\n"
            bErr = true
            bBDErr = true
        }
        // 生年月日・フォーマットエラー
        if(!bBDErr){
            let date = df.date(from: strDispBirth)
            if(date == nil){
                strErrMsg += "生年月日正しく入力してください。\n"
                bErr = true
                bBDErr = true
            }
        }
        // 生年月日・範囲外
        if(!bBDErr){
            // 上・下限
            let nowDate = roundDate(Date())
            let minDate = roundDate(Calendar.current.date(byAdding: .year, value: -120, to: Date())!)
            // 入力した生年月日
            let inputDate = roundDate(df.date(from: strDispBirth)!)
            
            if( !(minDate <= inputDate) || !(inputDate <= nowDate)){
                strErrMsg += "生年月日正しく入力してください。\n"
                bErr = true
                bBDErr = true
            }
        }
        // 性別
        if( nSex == nil )
        {
            strErrMsg += "性別を選択してください。\n"
            bErr = true
        }
        // 入所・退所・在宅
        if( nShelter == nil )
        {
            strErrMsg += "入所・退所・在宅を選択してください。\n"
            bErr = true
        }
        // 公表
        if( nRelease == nil )
        {
            strErrMsg += "公表可否を選択してください。\n"
            bErr = true
        }
        // 住所
        if( (strAddress?.count)! > 64 )
        {
            strErrMsg += "住所は６４文字までです。\n"
            bErr = true
        }
        if( !(strAddress?.canBeConverted(to: String.Encoding.shiftJIS))! )
        {
            strErrMsg += "住所に変換できない文字が含まれています。\n"
            bErr = true
        }
        // 怪我
        // チェック無し
        
        // 介護
        // チェック無し
        
        // 障がい
        // チェック無し
        
        // 妊産婦
        // チェック無し

        if bErr {
            DispAlert(title: "エラー", message: strErrMsg)
            return
        }
        
        //*************************************
        //　CSVデータ作成
        //*************************************
        CreateQRData(strPhone:strPhone, strName:strName,strBirth:strBirth,nSex:nSex,nShelter:nShelter,nRelease:nRelease,strAddress:strAddress,nInjury:nInjury,nCare:nCare,nFailure:nFailure,nPregnant:nPregnant)
        
        // 時刻は通常表示時は設定しない
        ViewController.s_strCsvTime = ""
        
        //*************************************
        //　表示データ作成
        //*************************************
        CreateDispString(strPhone:strPhone, strName:strDispName,strBirth:strDispBirth,nSex:nSex,nShelter:nShelter,nRelease:nRelease,strAddress:strAddress,nInjury:nInjury,nCare:nCare,nFailure:nFailure,nPregnant:nPregnant)

        // 表示内容を保存
        let timeFormatter = DateFormatter()
        timeFormatter.dateFormat = "yyyy-MM-dd' 'HH:mm:ss"
        timeFormatter.locale = Locale(identifier: "en_US_POSIX")
        let now = Date()
        // ViewController.s_strCsvはCreateDispStringで設定される
        let strPreData = timeFormatter.string(from: now) + "," + ViewController.s_strCsv + "," + bEncrypt
        
        let ud = UserDefaults.standard
        ud.set(strPreData, forKey: ViewController.KEY_PRE_DATA)

        ChangeMainView()
    }
    
    func roundDate(_ date: Date) -> Date{
        
        let calendar = Calendar(identifier: .gregorian)
        
        var components = DateComponents()
        components.year = calendar.component(.year, from: date)
        components.month = calendar.component(.month, from: date)
        components.day = calendar.component(.day, from: date)
        
        return calendar.date(from: components)!
    }
    
    @objc func preButtonEvent(sender: UIButton) {
        let ud = UserDefaults.standard
        let strPreVal = ud.string(forKey: ViewController.KEY_PRE_DATA)

        // ,で分割する
        let strItemList = strPreVal?.components(separatedBy: ",")
        
        var nItemCount = 0
        
        if( strItemList != nil )
        {
            nItemCount = (strItemList?.count)!
        }
        
        if nItemCount >= 13
        {
            // 時刻を表示する
            ViewController.s_strCsvTime = strItemList![0]
            
            // 電話番号
            let strPhone = strItemList![1]
            // 名前
            //  全角スペースを半角スペースに変換
            let strName = strItemList![2]
            let strDispName = strItemList![2].replacingOccurrences(of: "　", with: " ")
            // 年齢
            //let strAge = strItemList![3]
            // 生年月日
            //let strBirth = strItemList![3]
            // フォーマット変換 yyyyMMdd -> yyyy-MM-dd
            let strBirth = strItemList![3]
            let strDispBirth = ConvDateFormat(strDate: strItemList![3],
                                          fromFormat: "yyyyMMdd", toFormat: "yyyy-MM-dd")
            // 性別
            let nSex = ConvIntStrToIndexPath(strInt: strItemList![4])
            // 入所・退所・在宅
            let nShelter = ConvIntStrToIndexPath(strInt: strItemList![5])
            // 公表
            // 2019.04.03 Modify: Bug Fix 公表は許可、拒否の表示順と保存値が逆のため、反転させる
            //let nRelease = ConvIntStrToIndexPath(strInt: strItemList![6])
            var strRelease = "0"
            if( strItemList![6] == "0")
            {
                strRelease = "1"
            }
            let nRelease = ConvIntStrToIndexPath(strInt: strRelease)
            // 住所
            let strAddress = strItemList![7]
            // 怪我
            let nInjury = ConvIntStrToIndexPath(strInt: strItemList![8])
            // 介護
            let nCare = ConvIntStrToIndexPath(strInt: strItemList![9])
            // 障がい
            let nFailure = ConvIntStrToIndexPath(strInt: strItemList![10])
            // 妊産婦
            let nPregnant = ConvIntStrToIndexPath(strInt: strItemList![11])
            // 暗号化フラグ
            let bEncrypt = strItemList![12]
            if(bEncrypt == "1"){
                bEncryptFlag = true
            }else{
                bEncryptFlag = false
            }

            //*************************************
            //　CSVデータ作成
            //*************************************
            CreateQRData(strPhone:strPhone, strName:strName,strBirth:strBirth,nSex:nSex,nShelter:nShelter,nRelease:nRelease,strAddress:strAddress,nInjury:nInjury,nCare:nCare,nFailure:nFailure,nPregnant:nPregnant)
            
            //*************************************
            //　表示データ作成
            //*************************************
            CreateDispString(strPhone:strPhone, strName:strDispName,strBirth:strDispBirth,nSex:nSex,nShelter:nShelter,nRelease:nRelease,strAddress:strAddress,nInjury:nInjury,nCare:nCare,nFailure:nFailure,nPregnant:nPregnant)

            ChangeMainView()
        }
        else
        {
            // 前回データを削除し、ボタンを無効にする
            ud.set("", forKey: ViewController.KEY_PRE_DATA)
            btnBlk01_02.isEnabled = false
            btnBlk01_02.backgroundColor = UIColor.gray
            
            DispAlert(title: "エラー", message: "前回登録情報の読み出しに失敗しました。")
        }
    }
    
    @objc func dispButtonEvent(sender: UIButton) {
        if( lblBlk08_01.isHidden ) {
            DispDetailItem()
        }
        else {
            DispNonDetailItem()
        }
    }
    
    @objc func checkButtonEvent(sender: UIButton){
        sender.isSelected = !sender.isSelected
        bEncryptFlag = sender.isSelected
    }
    
    func ConvIntStrToIndexPath(strInt:String) -> IndexPath?
    {
        let nVal = Int(strInt)
        if nVal != nil
        {
            return IndexPath(row: nVal!, section: 0)
        }
        else
        {
            return nil
        }
    }
    
    func ConvDateFormat(strDate: String,fromFormat: String,toFormat: String) -> String{
        if(strDate == ""){
            return "";
        }
        
        // from date
        let fromFormatter = DateFormatter()
        fromFormatter.dateFormat = fromFormat
        fromFormatter.locale = Locale(identifier: "en_US_POSIX")
        let fromDate = fromFormatter.date(from: strDate)
        // toDate
        let toFormatter = DateFormatter()
        toFormatter.dateFormat = toFormat
        toFormatter.calendar = Calendar(identifier: .gregorian)
        toFormatter.locale = Locale(identifier: "en_US_POSIX")
        
        // convert
        let resultDate = toFormatter.string(from: fromDate!)
        
        return resultDate
    }
    
    // 指定されたデータからQRコードを作成
    func CreateQRData(strPhone:String!, strName:String!,strBirth:String!,nSex:IndexPath?,nShelter:IndexPath?,nRelease:IndexPath?,strAddress:String!,nInjury:IndexPath?,nCare:IndexPath?,nFailure:IndexPath?,nPregnant:IndexPath?)
    {
        var strCsvData:String = ""
        // 電話番号
        strCsvData += strPhone! + ","
        // 名前
        strCsvData += strName! + ","
        // 年齢
        //strCsvData += strAge! + ","
        // 生年月日
        strCsvData += strBirth! + ","
        // 性別
        if( nSex != nil)
        {
            strCsvData += (nSex?.row.description)!
        }
        strCsvData += ","
        // 入所・退所・在宅
        if( nShelter != nil)
        {
            strCsvData += (nShelter?.row.description)!
        }
        strCsvData += ","
        // 公表
        if( nRelease != nil)
        {
            var nWork = 0
            if nRelease?.row == 0
            {
                nWork = 1
            }
            strCsvData += (String(nWork))
        }
        strCsvData += ","
        // 住所
        strCsvData += strAddress! + ","
        // 怪我
        if( nInjury != nil)
        {
            strCsvData += (nInjury?.row.description)!
        }
        strCsvData += ","
        // 介護
        if( nCare != nil)
        {
            strCsvData += (nCare?.row.description)!
        }
        strCsvData += ","
        // 障がい
        if( nFailure != nil)
        {
            strCsvData += (nFailure?.row.description)!
        }
        strCsvData += ","
        // 妊産婦
        if( nPregnant != nil)
        {
            strCsvData += (nPregnant?.row.description)!
        }
        //2019/08/22 内外フラグ0固定を設定
        strCsvData += ",0"
        
        print(strCsvData)
        // NSString から NSDataへ変換
        ViewController.s_strCsv = strCsvData
        // 暗号化
        if( bEncryptFlag ){
            let encriptionStr = "1" + EncryptionCSV(strCSV: ViewController.s_strCsv)
            ViewController.s_strCsvData = encriptionStr.data(using: String.Encoding.shiftJIS)!
            ViewController.s_bEncryptFlg = true
        }else{
            ViewController.s_strCsvData = ("0" + strCsvData).data(using: String.Encoding.shiftJIS)!
            ViewController.s_bEncryptFlg = false
        }
    }
    
    // CSVの暗号化
    func EncryptionCSV(strCSV:String)->String{
//        let obj = EnCryptor_IF()
//        let str:String = strCSV
//        let enc:String = obj.encryptionData(str)! as String
        
        // Initialize
        // ====================
        let strKey = "zRcRMrWYdU2i3J4z"
        //let strKey = "dec"
        let keyData: NSData = strKey.data(using: .shiftJIS)! as NSData
        //let keyData: NSData = strKey.data(using: .utf8)! as NSData
        var keyBuffer = [UInt8](repeating: 0, count: keyData.length)
        keyData.getBytes(&keyBuffer, length: keyData.length  * MemoryLayout<UInt8>.size)
        //let umpKey = UnsafeMutablePointer(&keyBuffer)
        
        Initialize(&keyBuffer, (Int32)(keyBuffer.count))
        
        // Encode
        // ====================
        let str: String = strCSV
        let data: NSData = str.data(using: .shiftJIS)! as NSData
        //let data: NSData = str.data(using: .utf8)! as NSData
        var buffer = [UInt8](repeating: 0, count: data.length)
        data.getBytes(&buffer, length: data.length  * MemoryLayout<UInt8>.size)
//        var result = [UInt8](repeating: 0, count: data.length)
        
        let oCapacity = GetOutputLength(UInt32(buffer.count))
        var result = [UInt8](repeating: 0, count: Int(oCapacity))
        
        // Sample UnsafeMutablePointer
        // ----------
        //let umpBase = UnsafeMutablePointer(&buffer)
        //let umpResult = UnsafeMutablePointer(&result)
        //Encode(umpBase,umpResult,oCapacity)
        
        // Sample Address
        // ----------
        //Encode(&buffer, &result, oCapacity)
        Encode(&buffer,&result,UInt32(buffer.count))
        
        // conv byteArray & encode base64
        // ----------
        // Data -> EncodeString
        let encData : Data = Data(bytes: result)
        let enc : String = encData.base64EncodedString(options: [])
        
        print(enc)
        
        // Decode(確認用) - 成功無し 保留中
        // ====================
//        // decode base64
//        // ----------
//        /* ↓ .utf8に変換できず、decStrがnilになって止まる      */
//        /*   base64decodeはできる = resultの状態にはできるが、*/
//        /*   resultの状態をutf8に変換できないと思われる。      */
////        let decBase64Data = Data(base64Encoded: enc, options: [])!
////        let decStr : String = String(data: decBase64Data, encoding: .utf8)!
////        let decData: NSData = decStr.data(using: .utf8)! as NSData
//        /* utf8変換を挟まないでBase64Decodeのみ(resultのコピーを変換する) */
//        let decData : NSData = Data(base64Encoded: enc)! as NSData
//        /* resultをそのままわたす */
////        let decData : NSData = Data(bytes: result) as NSData
//        // decode
//        // ----------
//        //let encData = enc.data(using: .utf8)! as NSData
//        var DecBuffer = [UInt8](repeating: 0, count: decData.length)
//        decData.getBytes(&DecBuffer, length: data.length  * MemoryLayout<UInt8>.size)
//        var DecResult = [UInt8](repeating: 0, count: decData.length)
//        let dCap = GetOutputLength(UInt(DecBuffer.count))
//
//        // Sample UnsafeMutablePointer
//        // ----------
////        let umpDec = UnsafeMutablePointer(&DecBuffer)
////        let umpDecResult = UnsafeMutablePointer<UInt8>.allocate(capacity: Int(dCap))
////        Decode(umpDec, umpDecResult, dCap)
////        var dec: String = String(cString: umpDecode)
//
//        // Sample Address
//        // ----------
//        Decode(&DecBuffer, &DecResult, dCap)
//        /* DecResultをStringに直して復号できていれば成功 */
//        var dec = ""
//        /* DecResultをutf8に変換できない */
//        dec = String(bytes: DecResult, encoding: .utf8)!
//        //dec = String(data: Data(bytes: DecResult), encoding: .utf8)!
//        /* DecResultをbase64Decodeはできない/必要もない、はず */
////        dec = String(data: Data(base64Encoded: Data(bytes: DecResult))!, encoding: .utf8)
//        print(dec)
        
        // CBlowfish メモリ解放
        Decstractor()
        
        return enc
    }
    // CSVの暗号化
//    func EncryptionCSV(strCSV:String)->String
//    {
//        let bytes = [UInt8](strCSV.utf8)
//        let key = [UInt8]("YKo83n14SWf7o8G5".utf8)
//        let iv = String()
//        do{
//            let aes = try AES(key: key, blockMode: .ECB)
//            let encrypted = try aes.encrypt(bytes)
//            let encryptedData = NSData(bytes:encrypted, length:encrypted.count)
//            let sendData = NSMutableData(bytes: iv, length: iv.count)
//            sendData.append(encryptedData as Data)
//            let sendDataBase64 = sendData.base64EncodedString(options: NSData.Base64EncodingOptions())
//            return sendDataBase64
//        }catch let error as NSError{
//            debugPrint(error)
//            return ""
//        }
//    }
    
    // 指定されたデータから表示用データを作成
    func CreateDispString(strPhone:String!, strName:String!,strBirth:String!, nSex:IndexPath?,nShelter:IndexPath?,nRelease:IndexPath?,strAddress:String!,nInjury:IndexPath?,nCare:IndexPath?,nFailure:IndexPath?,nPregnant:IndexPath?)
    {
        // QR画面表示用
        var strDispItem:String = ""
        var nItemIdx = 0
        // 電話番号
        strDispItem += qrTitle[nItemIdx] + strPhone! + "\n"
        nItemIdx += 1
        // 名前
        strDispItem += qrTitle[nItemIdx] + strName! + "\n"
        nItemIdx += 1
        // 年齢
        //strDispItem += qrTitle[nItemIdx] + strAge! + "\n"
        //nItemIdx += 1
        // 生年月日
        strDispItem += qrTitle[nItemIdx] + strBirth! +  "\n"
        nItemIdx += 1
        // 性別
        strDispItem += qrTitle[nItemIdx]
        if( nSex != nil)
        {
            strDispItem += qrDataSex[(nSex?.row)!]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 入所・退所・在宅
        strDispItem += qrTitle[nItemIdx]
        if( nShelter != nil)
        {
            strDispItem += qrDataShelter[(nShelter?.row)!]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 公表
        strDispItem += qrTitle[nItemIdx]
        if( nRelease != nil)
        {
            var nWork = 0
            if nRelease?.row == 0
            {
                nWork = 1
            }
            strDispItem += qrDataRelease[nWork]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 住所
        strDispItem += qrTitle[nItemIdx] + strAddress! + "\n"
        nItemIdx += 1
        // 怪我
        strDispItem += qrTitle[nItemIdx]
        if( nInjury != nil)
        {
            strDispItem += qrDataInjury[(nInjury?.row)!]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 介護
        strDispItem += qrTitle[nItemIdx]
        if( nCare != nil)
        {
            strDispItem += qrDataCare[(nCare?.row)!]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 障がい
        strDispItem += qrTitle[nItemIdx]
        if( nFailure != nil)
        {
            strDispItem += qrDataFailure[(nFailure?.row)!]
        }
        strDispItem += "\n"
        nItemIdx += 1
        // 妊産婦
        strDispItem += qrTitle[nItemIdx]
        if( nPregnant != nil)
        {
            strDispItem += qrDataPregnant[(nPregnant?.row)!]
        }
        ViewController.s_strDispItem = strDispItem
    }
    
    @objc func clearButtonEvent(sender: UIButton) {
        // 電話番号
        txtPhone.text = ""
        // 名前
        txvName.text = ""
        txvGivenname.text = ""
        // 年齢
        //txtAge.text = ""
        // 生年月日
        txtYear.text = ""
        txtMonth.text = ""
        txtDate.text = ""
        // 性別
        let nSex = tblSexSel.indexPathForSelectedRow
        if( nSex != nil)
        {
            tblSexSel.deselectRow(at: nSex!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblSexSel, indexPath: nSex!, check: false)
        }
        // 入所・退所・在宅
        let nShelter = tblShelterSel.indexPathForSelectedRow
        if( nShelter != nil)
        {
            tblShelterSel.deselectRow(at: nShelter!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblShelterSel, indexPath: nShelter!, check: false)

            // 入所・退所・在宅のみ１行目をデフォルトで選択
            let indexPath = IndexPath(row: 0, section: 0)
            tblShelterSel.selectRow(at: indexPath, animated: false, scrollPosition: UITableView.ScrollPosition.none)
            CheckTableCell(tableView: tblShelterSel, indexPath: indexPath, check:true)
        }
        // 公表
        let nRelease = tblReleaseSel.indexPathForSelectedRow
        if( nRelease != nil)
        {
            tblReleaseSel.deselectRow(at: nRelease!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblReleaseSel, indexPath: nRelease!, check: false)
        }
        // 住所
        txvAddress.text = ""
        // 怪我
        let nInjury = tblInjurySel.indexPathForSelectedRow
        if( nInjury != nil)
        {
            tblInjurySel.deselectRow(at: nInjury!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblInjurySel, indexPath: nInjury!, check: false)
        }
        // 介護
        let nCare = tblCareSel.indexPathForSelectedRow
        if( nCare != nil)
        {
            tblCareSel.deselectRow(at: nCare!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblCareSel, indexPath: nCare!, check: false)
        }
        // 障がい
        let nFailure = tblFailureSel.indexPathForSelectedRow
        if( nFailure != nil)
        {
            tblFailureSel.deselectRow(at: nFailure!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblFailureSel, indexPath: nFailure!, check: false)
        }
        // 妊産婦
        let nPregnant = tblPregnantSel.indexPathForSelectedRow
        if( nPregnant != nil)
        {
            tblPregnantSel.deselectRow(at: nPregnant!, animated: false)
            // チェックマークを外す
            CheckTableCell(tableView: tblPregnantSel, indexPath: nPregnant!, check: false)
        }
        // 暗号化してQRコードに変換する
        btnEncrypt.isSelected = false
        bEncryptFlag = false

        ViewController.s_strCsv = ""
        ViewController.s_strDispItem = ""
        ViewController.s_strCsvData = nil
        ViewController.s_bEncryptFlg = false
    }
    
    func DispAlert(title:String, message:String) {
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
        present(alert, animated: true, completion: nil)
    }
    
    func ChangeMainView() {
        // QRコード確認画面 へ遷移するために Segue を呼び出す
        performSegue(withIdentifier: "toQrViewController",sender: nil)
    }
    
    func DispDetailItem() {
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size

        var fYOffset:CGFloat = 0
        var fXOffset:CGFloat = 0
        var fWidth:CGFloat = 0
        
        // 共通項目表示
        fXOffset = 0
        fYOffset += DispCommonItem()
        
        //*************************************
        // 任意項目表示
        //*************************************
        fXOffset = 12
        var size = lblDisp_01.bounds.size
        lblDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = btnDisp_01.bounds.size
        btnDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        btnDisp_01.setTitle("▲", for: UIControl.State.normal)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgDisp_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 住所
        //*************************************
        lblBlk08_01.isHidden = false
        txvAddress.isHidden = false
        imgBlk08_01.isHidden = false
        
        fXOffset = 12
        size = lblBlk08_01.bounds.size
        lblBlk08_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        txvAddress.sizeToFit()
        size = txvAddress.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        txvAddress.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 5
        size = imgBlk08_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk08_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 怪我
        //*************************************
        lblBlk09_01.isHidden = false
        tblInjurySel.isHidden = false
        imgBlk09_01.isHidden = false

        fXOffset = 12
        size = lblBlk09_01.bounds.size
        lblBlk09_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblInjurySel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblInjurySel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk09_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk09_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 介護
        //*************************************
        lblBlk10_01.isHidden = false
        tblCareSel.isHidden = false
        imgBlk10_01.isHidden = false
        
        fXOffset = 12
        size = lblBlk10_01.bounds.size
        lblBlk10_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblCareSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblCareSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk10_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk10_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 障がい
        //*************************************
        lblBlk11_01.isHidden = false
        tblFailureSel.isHidden = false
        imgBlk11_01.isHidden = false
        
        fXOffset = 12
        size = lblBlk11_01.bounds.size
        lblBlk11_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblFailureSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblFailureSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk11_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk11_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 妊産婦
        //*************************************
        lblBlk12_01.isHidden = false
        tblPregnantSel.isHidden = false
        imgBlk12_01.isHidden = false
        fXOffset = 12
        size = lblBlk12_01.bounds.size
        lblBlk12_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblPregnantSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblPregnantSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk12_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk12_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        fXOffset = 6
        size = btnEncrypt.bounds.size
        btnEncrypt.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 8
        fYOffset += size.height + 5
        size = imgBlk13_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk13_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = btnConfirmation.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        btnConfirmation.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        size = btnPrivacyPolicy.bounds.size
        fXOffset = scrnSize.width - size.width - 6
        btnPrivacyPolicy.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5

        //*************************************
        // navigationHeightではなく、safeAreaInsets.Topを使用するよう変更
        //let navigationHeight = self.navigationController?.navigationBar.frame.height
        var viewTop : CGFloat = 0
        if #available(iOS 11.0, *){
            viewTop = (UIApplication.shared.keyWindow?.safeAreaInsets.top) ?? 0
        }
        scrollView.contentSize = CGSize(width: scrnSize.width , height: fYOffset + viewTop)
    }

    func DispNonDetailItem() {
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size
        
        var fYOffset:CGFloat = 0
        var fXOffset:CGFloat = 0
        var fWidth:CGFloat = 0
        
        // 共通項目表示
        fXOffset = 0
        fYOffset += DispCommonItem()
        
        //*************************************
        // 任意項目表示
        //*************************************
        fXOffset = 12
        var size = lblDisp_01.bounds.size
        lblDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = btnDisp_01.bounds.size
        btnDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        btnDisp_01.setTitle("▼", for: UIControl.State.normal)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgDisp_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgDisp_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 住所
        //*************************************
        lblBlk08_01.isHidden = true
        txvAddress.isHidden = true
        imgBlk08_01.isHidden = true
        
        //*************************************
        // 怪我
        //*************************************
        lblBlk09_01.isHidden = true
        tblInjurySel.isHidden = true
        imgBlk09_01.isHidden = true
        
        //*************************************
        // 介護
        //*************************************
        lblBlk10_01.isHidden = true
        tblCareSel.isHidden = true
        imgBlk10_01.isHidden = true
        
        //*************************************
        // 障がい
        //*************************************
        lblBlk11_01.isHidden = true
        tblFailureSel.isHidden = true
        imgBlk11_01.isHidden = true
        
        //*************************************
        // 妊産婦
        //*************************************
        lblBlk12_01.isHidden = true
        tblPregnantSel.isHidden = true
        imgBlk12_01.isHidden = true

        //*************************************
        fXOffset = 6
        size = btnEncrypt.bounds.size
        btnEncrypt.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 8
        fYOffset += size.height + 5
        size = imgBlk13_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk13_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = btnConfirmation.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        btnConfirmation.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        size = btnPrivacyPolicy.bounds.size
        fXOffset = scrnSize.width - size.width - 6
        btnPrivacyPolicy.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // navigationHeightではなく、safeAreaInsets.Topを使用するよう変更
        //let navigationHeight = self.navigationController?.navigationBar.frame.height
        var viewTop : CGFloat = 0
        if #available(iOS 11.0, *){
            viewTop = (UIApplication.shared.keyWindow?.safeAreaInsets.top) ?? 0
        }
        scrollView.contentSize = CGSize(width: scrnSize.width , height: fYOffset + viewTop)
    }

    func DispCommonItem() -> CGFloat {
        // viewの大きさ設定
        let scrnSize = UIScreen.screens[0].bounds.size
        
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
        
        size = btnBlk01_01.bounds.size
        fXOffset = scrnSize.width - size.width - 10
        btnBlk01_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
//        size = lblBlk01_02.bounds.size
//        lblBlk01_02.frame = CGRect(x: fXOffset, y: fYOffset, width: scrnSize.width, height : size.height)
//
//        // 改行
//        fXOffset = 0
//        fYOffset += size.height + 5
        
        fXOffset = 8
        fYOffset += 5
        size = btnBlk01_02.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        btnBlk01_02.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 5
        size = imgBlk01_02.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk01_02.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 電話番号
        //*************************************
        fXOffset = 12
        size = lblBlk02_01.bounds.size
        lblBlk02_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk02_02.bounds.size
        lblBlk02_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk02_03.bounds.size
        lblBlk02_03.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height
        
        fXOffset = 12
        size = lblBlk02_04.bounds.size
        lblBlk02_04.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk02_05.bounds.size
        lblBlk02_05.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = txtPhone.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        txtPhone.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 5
        size = imgBlk02_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk02_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 名前 -> 姓
        //*************************************
        fXOffset = 12
        size = lblBlk03_01.bounds.size
        lblBlk03_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk03_02.bounds.size
        lblBlk03_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk03_03.bounds.size
        lblBlk03_03.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height
        
        fXOffset = 12
        size = lblBlk03_04.bounds.size
        lblBlk03_04.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk03_05.bounds.size
        lblBlk03_05.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        txvName.sizeToFit()
        size = txvName.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        txvName.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 5
        size = imgBlk03_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk03_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 名前
        //*************************************
        fXOffset = 12
        size = lblBlk14_01.bounds.size
        lblBlk14_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk14_02.bounds.size
        lblBlk14_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height
        
        fXOffset = 12
        size = lblBlk14_03.bounds.size
        lblBlk14_03.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk14_04.bounds.size
        lblBlk14_04.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        txvGivenname.sizeToFit()
        size = txvGivenname.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        txvGivenname.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 5
        size = imgBlk14_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk14_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        //*************************************
        // 年齢
        //*************************************
        fXOffset = 12
        size = lblBlk04_01.bounds.size
        lblBlk04_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_02.bounds.size
        lblBlk04_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_03.bounds.size
        lblBlk04_03.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height
        
        fXOffset = 12
        size = lblBlk04_04.bounds.size
        lblBlk04_04.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_05.bounds.size
        lblBlk04_05.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        
//        // 年齢 は生年月日に変更
//        fXOffset = 6
//        size = txtAge.bounds.size
//        fWidth = scrnSize.width - ( fXOffset * 2 )
//        txtAge.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
//        fXOffset += size.width
        
        fXOffset = 12
        /* 高さを揃えるため、paddingの分大きいTextFieldに高さを合わせる */
        let fBirthHeight = txtYear.bounds.size.height
        // 生年月日
        size = lblBlk04_06.bounds.size
        lblBlk04_06.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: fBirthHeight)
        fXOffset += size.width
        
        size = txtYear.bounds.size
        txtYear.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_07.bounds.size
        lblBlk04_07.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: fBirthHeight)
        fXOffset += size.width
        
        size = txtMonth.bounds.size
        txtMonth.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_08.bounds.size
        lblBlk04_08.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: fBirthHeight)
        fXOffset += size.width
        
        size = txtDate.bounds.size
        txtDate.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk04_09.bounds.size
        lblBlk04_09.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: fBirthHeight)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 10
        
        fXOffset = 5
        size = imgBlk04_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk04_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 性別
        //*************************************
        fXOffset = 12
        size = lblBlk05_01.bounds.size
        lblBlk05_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk05_02.bounds.size
        lblBlk05_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height
        
        fXOffset = 12
        size = lblBlk05_03.bounds.size
        lblBlk05_03.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk05_04.bounds.size
        lblBlk05_04.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblSexSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblSexSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk05_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk05_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 入所・退所・在宅
        //*************************************
        fXOffset = 12
        size = lblBlk06_01.bounds.size
        lblBlk06_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk06_02.bounds.size
        lblBlk06_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblShelterSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblShelterSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 8
        size = imgBlk06_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk06_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        //*************************************
        // 公表
        //*************************************
        fXOffset = 12
        size = lblBlk07_01.bounds.size
        lblBlk07_01.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        size = lblBlk07_02.bounds.size
        lblBlk07_02.frame = CGRect(x: fXOffset, y: fYOffset, width: size.width, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 6
        size = tblReleaseSel.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        tblReleaseSel.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        fXOffset += size.width
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        fXOffset = 12
        size = lblBlk07_03.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        lblBlk07_03.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height: size.height)
        lblBlk07_03.sizeToFit()
        size = lblBlk07_03.bounds.size

        // 改行
        fXOffset = 0
        fYOffset += size.height + 10

        fXOffset = 8
        size = imgBlk07_01.bounds.size
        fWidth = scrnSize.width - ( fXOffset * 2 )
        imgBlk07_01.frame = CGRect(x: fXOffset, y: fYOffset, width: fWidth, height : size.height)
        
        // 改行
        fXOffset = 0
        fYOffset += size.height + 5
        
        return fYOffset
    }

    // セルが選択された時に呼び出される
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        CheckTableCell(tableView: tableView, indexPath: indexPath, check: true)
    }
    
    // セルの選択が外れた時に呼び出される
    func tableView(_ tableView: UITableView, didDeselectRowAt indexPath: IndexPath) {
        CheckTableCell(tableView: tableView, indexPath: indexPath, check: false)
    }

    func CheckTableCell(tableView: UITableView, indexPath: IndexPath, check:Bool) {
        let cell = tableView.cellForRow(at:indexPath)
        
        if( check )
        {
            // チェックマークを入れる
            cell?.accessoryType = .checkmark
        }
        else
        {
            // チェックマークを外す
            cell?.accessoryType = .none
        }
    }
    
    // キーボード以外をタッチした時にキーボードをしまう
    @IBAction func tapScreen(sender: UITapGestureRecognizer){
        view.endEditing(true)
        
        // レイアウトの更新
        AutoLayout()
    }

    // レイアウトの更新
    func AutoLayout() {
        if( lblBlk08_01.isHidden ) {
            DispNonDetailItem()
        }
        else {
            DispDetailItem()
        }
    }
    
    // セルの数を返す
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if( tableView.tag == 1)
        {
            return dataSex.count
        }
        else if( tableView.tag == 2)
        {
            return dataShelter.count
        }
        else if( tableView.tag == 3)
        {
            return dataRelease.count
        }
        else if( tableView.tag == 4)
        {
            return dataInjury.count
        }
        else if( tableView.tag == 5)
        {
            return dataCare.count
        }
        else if( tableView.tag == 6)
        {
            return dataFailure.count
        }
        else
        {
            return dataPregnant.count
        }
    }
    
    func tableView(_ table: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if( table.tag == 1)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "SexCell", for: indexPath)
            cell.textLabel?.text = "\(dataSex[indexPath.row])"
            
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else if( table.tag == 2)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "ShelterCell", for: indexPath)
            //cell.textLabel?.text = "\(dataShelter[indexPath.row])"
            // Tag番号 101 で UILabel インスタンスの生成
            let label1 = cell.viewWithTag(101) as! UILabel
            label1.text = "\(dataShelter[indexPath.row])"
            if( indexPath.row == 0 )
            {
                // Tag番号 102 で UILabel インスタンスの生成
                let label2 = cell.viewWithTag(102) as! UILabel
                label2.text = "Click here"
            }
            else if( indexPath.row == 2 )
            {
                // Tag番号 102 で UILabel インスタンスの生成
                let label2 = cell.viewWithTag(102) as! UILabel
                label2.text = ""
                
                label1.numberOfLines = 3
                label1.sizeToFit()
            }
            else {
                // Tag番号 102 で UILabel インスタンスの生成
                let label2 = cell.viewWithTag(102) as! UILabel
                label2.text = ""
            }
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else if( table.tag == 3)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "ReleaseCell", for: indexPath)
            //cell.textLabel?.text = "\(dataRelease[indexPath.row])"
            // Tag番号 101 で UILabel インスタンスの生成
            let label1 = cell.viewWithTag(101) as! UILabel
            label1.text = "\(dataRelease[indexPath.row])"
                
            if( indexPath.row == 0 )
            {
                // Tag番号 102 で UILabel インスタンスの生成
                let label2 = cell.viewWithTag(102) as! UILabel
                label2.text = "Click here"
            }
            else
            {
                // Tag番号 102 で UILabel インスタンスの生成
                let label2 = cell.viewWithTag(102) as! UILabel
                label2.text = ""
            }
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else if( table.tag == 4)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "InjuryCell", for: indexPath)
            cell.textLabel?.text = "\(dataInjury[indexPath.row])"
            
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else if( table.tag == 5)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "CareCell", for: indexPath)
            cell.textLabel?.text = "\(dataCare[indexPath.row])"
            
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else if( table.tag == 6)
        {
            let cell = table.dequeueReusableCell(withIdentifier: "FailureCell", for: indexPath)
            cell.textLabel?.text = "\(dataFailure[indexPath.row])"
            
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
        else
        {
            let cell = table.dequeueReusableCell(withIdentifier: "PregnantCell", for: indexPath)
            cell.textLabel?.text = "\(dataPregnant[indexPath.row])"
            
            // セルが選択された時の背景色を消す
            cell.selectionStyle = UITableViewCell.SelectionStyle.none
            return cell
        }
    }
    
    func tableView(_ table: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if( table.tag == 2)
        {
            if( indexPath.row == 2 )
            {
                return 90
            }
            else
            {
                return 44
            }
        }
        else
        {
            return 44
        }
    }
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        // レイアウトの更新
        AutoLayout()
        return true
    }
    
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString text: String) -> Bool {
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
                if charTxt.range(of: "^[0-9]+$", options: .regularExpression, range: nil, locale: nil) == nil {
                    return false
                }
            }
            // 全てが数値の時は入力受付
            return true
        }
    }

    @objc private func textFieldDidChange(notification: NSNotification) {
        let txtField = notification.object as! UITextField
        let textFieldString = txtField
        // 電話番号
        if( txtField.tag == 1)
        {
            if let text = textFieldString.text {
                if text.count > 12 {
                    txtField.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:12)])
                }
            }
        }
        // 年齢
        else if( txtField.tag == 3)
        {
            if let text = textFieldString.text {
                if text.count > 3 {
                    txtField.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:3)])
                }
            }
        }
    }
    
    func textViewShouldReturn(_ textView: UITextView) -> Bool {
        textView.resignFirstResponder()
        // レイアウトの更新
        AutoLayout()
        return true
    }
    
    func textView(_ textView: UITextView, shouldChangeTextIn range: NSRange, replacementText text: String) -> Bool {
        // 改行が入力されたらキャンセルして終了
        if text == "\n" {
            textView.resignFirstResponder() //キーボードを閉じる
            // レイアウトの更新
            AutoLayout()
            return false
        }
        // ペーストなどで改行を含む文字列を貼られた場合はキャンセルする
        else if( text.index(of: "\n") != nil ) {
            return false
        }
        // ,が入力されたらキャンセル
        else if text == "," {
            return false
        }
        // ペーストなどで,を含む文字列を貼られた場合はキャンセルする
        else if( text.index(of: ",") != nil ) {
            return false
        }
        return true
    }
    
    @objc private func textViewDidChange(notification: NSNotification) {
        let txtView = notification.object as! UITextView
        let textFieldString = txtView
        // 姓
        if( txtView.tag == 2)
        {
            if let text = textFieldString.text {
                if text.count > 12 {
                    textFieldString.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:12)])
                }
            }
        }
        // 住所
        else if( txtView.tag == 4)
        {
            if let text = textFieldString.text {
                if text.count > 64 {
                    textFieldString.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:64)])
                }
            }
        }
        // 名
        else if( txtView.tag == 5){
            if let text = textFieldString.text {
                if text.count > 12 {
                    textFieldString.text = String(text[text.startIndex..<text.index(text.startIndex, offsetBy:12)])
                }
            }
        }
    }
    
    @objc private func changedDate(sender:UIDatePicker){
//        let formatter = DateFormatter()
//        // year
//        formatter.dateFormat = "yyyy"
//        txtYear.text = formatter.string(from: sender.date)
//        // month
//        formatter.dateFormat = "MM"
//        txtMonth.text = formatter.string(from: sender.date)
//        // date
//        formatter.dateFormat = "dd"
//        txtDate.text = formatter.string(from: sender.date)
        setText_Date(sender: sender)
    }
    @objc private func onClick_done(){
        self.view.endEditing(true)
        setText_Date(sender: datePicker)
    }
    private func setText_Date(sender:UIDatePicker){
        let formatter = DateFormatter()
        formatter.locale = Locale(identifier: "en_US_POSIX")
        formatter.calendar = Calendar(identifier: .gregorian)
        // year
        formatter.dateFormat = "yyyy"
        txtYear.text = formatter.string(from: sender.date)
        // month
        formatter.dateFormat = "MM"
        txtMonth.text = formatter.string(from: sender.date)
        // date
        formatter.dateFormat = "dd"
        txtDate.text = formatter.string(from: sender.date)
    }
    @objc private func onClick_cancel(){
        txtYear.text = ""
        txtMonth.text = ""
        txtDate.text = ""
        
        self.view.endEditing(true)
    }
    
    @objc func privacyPolicyButtonEvent(sender: UIButton) {
        UIApplication.shared.open(URL(string: URL_PRIVACY_POLICY)!)
    }
}

