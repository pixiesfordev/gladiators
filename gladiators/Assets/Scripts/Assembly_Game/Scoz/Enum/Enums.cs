namespace Scoz.Func {

    public enum MyScene {
        StartScene,//開始場景
        LobbyScene,//大廳場景
        BattleScene,//戰場場景
        BattleSimulationScene,//戰場模擬場景
        //這裡有追加Scene，若是正式版需要Build的話，BuildCommand.cs中的BuildScenes清單也要一併追加，Jenkins自動包檔才會包到
        TrainVigor,//(動態載入，不會加到BuildSetting中)體力訓練場景
        TrainRock,
        TrainCave,
        TrainHunt,
        Cuisine,
    }
    public enum EnvVersion {
        Dev,//開發版(內部開發使用)
        Test,//測試版(對外提供測試使用)
        Release//正式版
    }
    public enum MyAudioType {
        Sound,
        Music,
        Voice,
    }
    public enum Language {
        EN,
        TW,
        CH,
        JP,
    }
    public enum BuffType {
        Buff = 0,
        None = 1,
        Debuff = 2,
    }
    public enum FullScreen {
        Off,
        On,
    }
    public enum UsePlatformIcon {
        None,
        Facebook,
    }
    public enum AuthType {
        NotSigninYet,//尚未登入
        GUEST,//訪客註冊
        GOOGLE,
        APPLE,
        X,
        TIKTOK,
        WECHAT,
    }
    public enum ThirdPartLink {
        Facebook,
        Google,
        Apple,
    }
    public enum Direction {
        Top,
        Right,
        Bottom,
        Left,
    }
    public enum RightLeft {
        Right = 1,
        Left = -1,
    }
    public enum Anchor {
        TopLeft,//左上
        MiddleLeft,//左中
        BottomLeft,//左下
        TopCenter,//中上
        MiddleCenter,//中心
        BottomCenter,//中下
        TopRight,//右上
        MiddleRight,//右中
        BottomRight,//右下
    }
    public enum ControlWay {
        Keyboard,
        Cursor,
        ScreenTouch,
    }
    public enum Operator {
        Plus,
        Minus,
        Times,
        Divided,
        Equal
    }
    public enum Operation {
        EqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
    }
    public enum OrderType {
        Descend,//降序
        Ascend,//升序
    }
    public enum FontType {
        Title,
        Content,
    }
    public enum BuyCount {
        One,
        Ten,
    }
    public enum Gender {
        Female,
        Male,
    }
    public enum AddValueNumber {
        One,
        Ten,
    }
    public enum AvailableAxis {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ,
    }
    public enum BigNumberType {
        BelowK,
        K,
        M,
        B,
        T,
        Qua,
        Qui,
        Sex,
        Sep,
        Oct,
        Non,
        Dec,
        Und,
        Duo,
        Tre,
        QuaDec,
        QuiDec,
        SexDec,
        SepDec,
        OctDec,
        NovDec,
        Vig,
        Cen,
    }
    public enum BigNumberType2 {
        BelowK,
        K,
        Million,
        Billion,
        Trillion,
        Quadrillion,
        Quintillion,
        Sextillion,
        Septillion,
        Octillion,
        Nonillion,
        Decillion,
        Undecillion,
        Duodecillion,
        Tredecillion,
        Quattuordecillion,
        Quindecillion,
        Sexdecillion,
        Septendecillion,
        Octodecillion,
        Novemdecillion,
        Vigintillion,
        Centillion,
    }
}