# コーディング規約

## テストクラス/メソッドの命名

テストクラスは以下の命名方法とする

```
[TestClass]
public class Given_[前提条件] {
    ...
}
```

テストメソッドは以下の命名方法とする
```
[TestMethod]
public void When_[発生条件]_Then_[振る舞い]() {

}
```
