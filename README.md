# Project Description
ReactiveMVVM is MVVM pattern, it's achieved by Microsoft Reactive Extensions. Can used in silverlight, WPF and WP7. ReactiveMVVM makes it easier for you to develop multithreading program in Silverlight, WPF and WP7 project. To be good work with it, you need know Rx framework.


## Use State:
### Example 1 property change:

```C#
public class Example1 : ViewModelBase{
    string _Userid;
    /// <summary>
    /// person infomation of owner.
    /// </summary>
    public string Userid
    {
        get { return _Userid; }
        set { this.RaiseAndSetIfChanged(x => x.Userid, ref _Userid, value, *true*); } // true, broadcast property change message.
    }
    //if the property changed to do...
    this.ObservableProperty(x => x.Useid)
        .DistinctUntilChanged()
        //can do work in any scheduler by ObserveOn
        .Subscribe(chg =>
        {
            if (!string.IsNullOrEmpty(chg.NewValue))
            {
                //TODO: do you work.
            }
        });
}
```

### Example 2 Subscribe Command:
#### 1
```C#
this.GetPinCodeCommand = new RxCommand();
...
 this.GetPinCodeCommand
    .Subscribe( x =>
    {
        var strle = OAuthHelper.GetSetting("PinCodeLocal");
        strle = strle.Replace(" ", "");
        var htmlStr = ((WebBrowser)x).SaveToString();
        htmlStr = htmlStr.Replace(" ", "");
        ......
```
#### 2
```C#
this.GetPinCodeCommand = new RxCommand(dowork , canExecute,onError,afterCompleteWork, scheduer);
```
### Example 3 Messager:
```C#
//Subscribe message action
Messenger.Default.Register<QueryMessage<UserInfo>>(
    "GetUser",
     qm => OAuthHelper.UserDataBase
        .SubscribeQueryData(
            lq => lq.Id == qm.Id,
            OAuthHelper.CreateClient("USER_PROFILE", new ParameterCollection { { "user_id", qm.Id } })
            .QueryingRemote(sf => sf.ReadUserProfileInfo()),
            qm.Callback,
            a => a.Id
        )
        ,
        scheduler: Scheduler.ThreadPool);

public class Example3 : ViewModelBase{
    ....
    someMethod(){
        //Send meesage
        Messenger.Send(new QueryMessage<UserInfo>
            {
                Id = userid,
                //Sender = this,
                Name = "GetUser",
                Callback = x =>
                {
                    CurrentUser.Person = x;
                    if (string.IsNullOrWhiteSpace(OAuthHelper.Account.ScreenName))
                    {
                        OAuthHelper.Account.ScreenName = x.ScreenName;
                    }
                }
            });
    }
    ...
```