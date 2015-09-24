# TTUIFramework
UI Framework For Unity3D
>`Simple` and Powerful!
>>UI like a book,all you see are `pages`.you can easy jumpto between pages.
>>>When create a new page ,only need to override some function!
>>>>Simple!Simple!Simple! (>,< important thing need 3 times.haha...)
>>>>>[THINKING IN IMAGE](https://www.processon.com/embed/55ee822fe4b0f2eb8914c311)



>>>>>[Thanks This Guy](https://github.com/MrNerverDie/Unity-UI-Framework)

## Usage
1:how to show one page?  
Because how to load ui.prefab depend on yours.

#### Sync load & show ui api:  
this style no need page instance. only page target type.  
`TTUIPage.ShowPage<T>()`  
`TTUIPage.ShowPage<T>(object pageData)`  

another style is show target page instance with name. this is easy way for Lua use.  
`TTUIPage.ShowPage(string pageName,TTUIPage pageInstance)`  
`TTUIPage.ShowPage(string pageName,TTUIPage pageInstance,object pageData)`  

NOTE:`object pageData` is your page's data instance. send or not depend on your page `Refresh()` logic.  

#### Async load & show ui api:  
`TTUIPage.ShowPage<T>(Action callback)`  
`TTUIPage.ShowPage<T>(Action callback,object pageData)`  
`TTUIPage.ShowPage(string pageName,TTUIPage pageInstance,Action callback)`  
`TTUIPage.ShowPage(string pageName,TTUIPage pageInstance,Action callback,object pageData)`  

### How to create new page?  
all your new page should inherit from `TTUIPage`  
5 virtual functions should implement base on your page needed.  
`virtual void Awake()` this is once when Instantiate.  
`virtual void Refresh()` this is call eachtime `ShowPage`  
`virtual void Active()` this is how to active this page,default is this.gameObject.SetActive(true)  
`virtual void Hide()` this is how to deactive this page,default is this.gameObject.SetActive(false)  
`virtual void Close()` NOTE:this is how to `Destroy` your page! when `return` close means clear DATA!!  

## Write Your MVC page!  
in Sample folder's TTSkillPage is simple to use MVC logic. clearly your `Refresh()` get your data. and your data only data! 
