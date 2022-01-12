//
//  CustomNavigationBar.swift
//  Q_ANIP
//
//  Created by user1 on 2018/08/07.
//  Copyright © 2018年 L&A. All rights reserved.
//

import Foundation
import UIKit

@IBDesignable class CustomNavigationBar: UINavigationBar {
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        setup()
    }
    
    func setup(){
        self.barTintColor = UIColor.red
        self.isTranslucent = false
        //self.setBackgroundImage(UIImage(), for: .default)
    }
    
    override func layoutSubviews() {
        super.layoutSubviews()
        // iOS11のみ
        if #available(iOS 11.0, *) {
            for subview in self.subviews {
                let stringFromClass = NSStringFromClass(subview.classForCoder)
                if stringFromClass.contains("BarBackground"){
                    let statusBarHeight = UIApplication.shared.statusBarFrame.height
                    let point = CGPoint(x: 0, y: 0 - statusBarHeight)
                    subview.frame = CGRect(origin: point, size: sizeThatFits(self.bounds.size))
                }
            }
        }
    }
    override func sizeThatFits(_ size: CGSize) -> CGSize {
        var newSize = super.sizeThatFits(size)
        let addHeight: CGFloat = -44.0
        // notch分
        var topInset : CGFloat = 0
        if #available(iOS 11.0, *){
            topInset = superview?.safeAreaInsets.top ?? 0
        }
        
        newSize.height += addHeight + topInset
        
        return newSize
    }
}
