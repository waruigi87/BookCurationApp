# BookCurationApp

## 1. 目的 / 背景
- **背景**  
  - 日本の英語教育市場は年平均 17% で成長。良質な英語原書ニーズが増加。  
  - 既存 EC は「在庫検索」に留まり、**学習者に最適な一冊を探す体験**が不十分。  
- **目的**  
  - *教師・保護者* が “次に読むべき児童書” を **5 秒以内** で見つけられることを技術的に実証する。  
  - サービス α版に向けた基盤（検索 API＋フロント）を 3.5 h 以内で構築する。  

---

## 2. 想定ユーザー
| ペルソナ | 属性 | ニーズ |
|----------|------|--------|
| **教師** | インターナショナルスクール / 英会話講師 | レベル・興味に合う本を素早く推薦したい |
| **保護者** | 小学生の保護者、英語学習熱心 | 子どもが続けて読みたくなる本を知りたい |

---

## 3. 課題
1. 児童書タイトルが多すぎて選定に時間がかかる。  
2. 日本語 UI でまとまった洋書検索サービスが少ない。  
3. 価格比較や在庫確認は別サイトを横断する必要がある。  

---

## 4. 提供価値 / 解決策
- **検索キーワード／ISBN ひとつで** → *最低 3 冊* を **横スクロールカード**で提示。  
- **Blazor WASM** によりページ遷移ゼロの高速 UX。  
- **Google Books API** のみで PoC を完結、後続フェーズで価格比較を統合。  

---

## 5. 機能要件
| 優先度 | 機能 | 説明 |
|--------|------|------|
| **Must** | 書籍検索フォーム | キーワード／ISBN を入力して検索トリガー |
| Must | `/api/v1/books` API | Google Books → JSON(title, authors, thumbnail, isbn) 返却 |
| Must | カード UI | 各書籍を画像＋タイトル＋⭐ボタンで表示 |
| **Nice** | お気に入り保存 (LocalStorage) | ⭐クリックで端末内に一時保存 |
| Nice | メール登録モーダル | お気に入りが 3 冊以上でポップアップ |

---

## 6. 非機能要件
- **パフォーマンス**: 検索→表示まで総計 ≤5 s、API 平均応答 ≤1 s  
- **可用性**: Amplify Hosting 99.9%（AWS SLA 準拠）  
- **セキュリティ**: PoC は匿名利用・公開キー無し。CORS ワイルドカードを本番前に制限予定。  
- **拡張性**: API Gateway 階層 `/api/v1/*` を採用し、price・user リソースを後付け可能。  

---

## 7. KPI (PoC)
| 指標 | ゴール |
|------|-------|
| 検索完了率 | 80 % 以上 |
| 3 冊以上提示率 | 90 % 以上 |
| 表示完了時間 | p95 < 5 s |

---

## 8. 画面設計（主要）
1. **SearchPage**  
   - 検索バー  
   - 結果カード横スクロール  
2. **CardComponent**  
   - サムネイル / タイトル / ⭐ボタン  
3. **(Nice) FavoriteModal**  
   - メール登録フォーム  

---

## 9. 機能仕様フロー
```mermaid
flowchart TD
    A[ユーザー入力] -->|q パラメータ| B[/api/v1/books GET/]
    B --> C{ISBN?}
    C -- yes --> D[Google Books ISBN 検索]
    C -- no --> E[Google Books キーワード検索]
    D & E --> F[JSON 4フィールド]
    F --> G[Blazor Card 描画]
    G --> H{⭐クリック}
    H -- 保存 --> I[LocalStorage addFavorite()]
10. 工程 & 役割
週	タスク	担当	成果物
0 (本日)	Amplify + Blazor 初期デプロイ	Dev	空テンプレ公開
0	API Gateway & Lambda(nodejs22) 作成	Dev	/api/v1/books 動作
0	Card UI & GenSpark テーマ適用	Dev / Designer	検索→表示完了
1	お気に入り保存 & モーダル (Nice)	Dev	LocalStorage 実装
1	DoD テスト & 振り返り	PM	受入レポート

11. リスク & 対応
リスク	影響	対策
Google Books レート制限	検索失敗	キャッシュ or API Key 複数化
CORS 設定漏れ	検索不可	デプロイ時チェックリストに追加
Node->Blazor CORS遅延	UX劣化	JSON最小化・gzip 有効化

12. 参考情報
ランタイム: Blazor .NET 8 (LTS)／Lambda nodejs22.x

拡張予定: price API → Open Library / PA-API、認証 → Cognito Guest/Registered
