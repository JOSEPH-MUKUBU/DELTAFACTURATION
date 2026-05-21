# ⚡ DeltaFacturation

> **Système de Gestion Commerciale & Facturation** — Application web Blazor complète pour la gestion des factures, clients, produits, et analyses fiscales selon la législation tunisienne.

---

## 📋 Table des Matières

- [Aperçu](#-aperçu)
- [Fonctionnalités](#-fonctionnalités)
- [Architecture du Projet](#-architecture-du-projet)
- [Technologies Utilisées](#-technologies-utilisées)
- [Prérequis](#-prérequis)
- [Installation & Configuration](#-installation--configuration)
- [Lancement de l'Application](#-lancement-de-lapplication)
- [Structure des Dossiers](#-structure-des-dossiers)
- [Modèle de Données](#-modèle-de-données)
- [Authentification & Rôles](#-authentification--rôles)
- [Modules Fonctionnels](#-modules-fonctionnels)
- [Composants UI](#-composants-ui)

---

## 🌟 Aperçu

**DeltaFacturation** est une application web de gestion commerciale et de facturation destinée aux entreprises tunisiennes. Elle permet de gérer l'intégralité du cycle de vente : de la création des produits jusqu'à la génération des factures avec calcul automatique de la TVA et du timbre fiscal.

L'application est construite sur une architecture en couches (Clean Architecture) avec **Blazor Web App** sur .NET 10, offrant une interface utilisateur interactive, des graphiques analytiques temps réel, et des outils d'export comptable.

---

## ✨ Fonctionnalités

### 📄 Facturation
- Création, édition et suppression de factures
- Numérotation automatique des factures (`FAC-XXXXXX`)
- Calcul automatique HT, TVA multi-taux, TTC
- Calcul du timbre fiscal configurable
- Statuts de facture : **Brouillon**, **Validée**, **Annulée**
- Vue détaillée avec impression / export PDF
- Filtres avancés par période, statut, client

### 👥 Clients
- Gestion complète du répertoire clients
- Champs : Nom, Email, Téléphone, Adresse, Matricule Fiscal
- Historique des achats par client

### 📦 Produits & Catalogue
- Gestion du catalogue produits avec codes uniques
- Suivi du stock en temps réel (stock actuel vs seuil minimal)
- Taux de TVA par produit (7%, 13%, 19%)
- Activation / désactivation des produits

### 📊 Tableaux de Bord & Analytique
- **Dashboard principal** : KPIs, CA mensuel, top clients, top produits
- **Analytique Ventes** : Graphiques d'évolution du CA, répartition par période
- **Analytique Fiscalité** : TVA collectée par taux, évolution mensuelle, timbres fiscaux
- **Prévisions** : Projections basées sur l'historique des ventes

### 🔔 Alertes Stock
- Détection automatique des produits sous le seuil minimal
- Vue dédiée avec indicateurs visuels

### 📤 Export & Comptabilité
- Export CSV au format comptable
- Prévisions financières exportables

### ⚙️ Paramètres Système
- Configuration du montant du timbre fiscal
- Visualisation des taux de TVA applicables

---

## 🏗️ Architecture du Projet

Le projet suit une **Architecture en Couches (Clean Architecture)** stricte avec 4 projets séparés :

```
DeltaFacturation/
├── Facturation.Core/           ← Couche Domaine (entités + interfaces)
├── Facturation.Infrastructure/ ← Couche Infrastructure (EF Core + MySQL)
├── Facturation.Services/       ← Couche Métier (logique applicative)
└── Facturation.Web/            ← Couche Présentation (Blazor Web App)
```

### Flux de dépendances

```
Web → Services → Infrastructure → Core
         ↓                           ↑
      (utilise)               (implémente)
```

- **Core** : Aucune dépendance externe — contient uniquement les entités et les contrats (interfaces)
- **Infrastructure** : Implémente les repositories, configure EF Core et MySql
- **Services** : Contient la logique métier, orchestre les repositories
- **Web** : Couche Blazor — injecte et utilise les services

---

## 🛠️ Technologies Utilisées

| Couche | Technologie | Version |
|--------|------------|---------|
| Framework | .NET / ASP.NET Core | 10.0 |
| UI | Blazor Web App (Interactive Server) | .NET 10 |
| UI Components | MudBlazor | Dernière |
| Charts & Graphiques | Radzen Blazor | Dernière |
| Icons | Bootstrap Icons | 1.x |
| ORM | Entity Framework Core | 9.0 |
| Base de données | MySQL (via Pomelo) | 9.0 |
| Police de texte | Google Fonts (Poppins, Inter) | — |
| Notifications | MudBlazor Snackbar | — |

---

## 📦 Prérequis

Avant de lancer l'application, assurez-vous d'avoir installé :

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (≥ 8.0) ou MariaDB
- Un navigateur moderne (Chrome, Edge, Firefox)

Vérifier les installations :
```bash
dotnet --version   # Doit afficher 10.x.x
mysql --version    # Doit afficher 8.x.x ou +
```

---

## ⚙️ Installation & Configuration

### 1. Cloner le projet

```bash
git clone <url-du-depot>
cd DeltaFacturation
```

### 2. Configurer la base de données

Créez une base de données MySQL vide :

```sql
CREATE DATABASE FacturationDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Modifier la chaîne de connexion

Éditez le fichier [`Facturation.Web/appsettings.json`](Facturation.Web/appsettings.json) :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FacturationDB;Uid=root;Pwd=VOTRE_MOT_DE_PASSE;Port=3306;"
  }
}
```

> **Note** : Remplacez `root` et `VOTRE_MOT_DE_PASSE` par vos identifiants MySQL.

### 4. Appliquer les migrations

```bash
dotnet ef database update --project Facturation.Infrastructure --startup-project Facturation.Web
```

Cela crée automatiquement toutes les tables dans la base de données.

### 5. Restaurer les dépendances

```bash
dotnet restore
```

---

## 🚀 Lancement de l'Application

```bash
dotnet run --project Facturation.Web
```

Accédez à l'application dans votre navigateur :

```
http://localhost:5007
```

---

## 🔐 Authentification & Rôles

L'application dispose d'un système d'authentification intégré avec deux comptes préconfigurés :

| Rôle | Email | Mot de passe |
|------|-------|--------------|
| **Administrateur** | `admin@delta.com` | `admin123` |
| **Commercial** | `commercial@delta.com` | `commercial123` |

> Le système d'authentification est géré par `IAuthenticationService` enregistré comme **Singleton** pour garantir la persistance de la session entre les pages.

---

## 📁 Structure des Dossiers

```
DeltaFacturation/
│
├── Facturation.Core/
│   ├── Entities/
│   │   ├── Client.cs           ← Entité Client
│   │   ├── Facture.cs          ← Entité Facture
│   │   ├── LigneFacture.cs     ← Ligne d'une facture (produit + quantité)
│   │   ├── Produit.cs          ← Entité Produit du catalogue
│   │   └── Parametre.cs        ← Paramètres système (timbre fiscal, TVA)
│   └── Interfaces/
│       ├── IRepository.cs      ← Interface générique CRUD
│       └── IFactureRepository.cs ← Interface spécialisée factures
│
├── Facturation.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs  ← Contexte EF Core + seed data
│   ├── Repositories/
│   │   ├── Repository.cs            ← Implémentation générique
│   │   └── FactureRepository.cs     ← Requêtes spécialisées factures
│   └── Migrations/                  ← Migrations EF Core (auto-générées)
│
├── Facturation.Services/
│   └── Services/
│       ├── AnalyseService.cs    ← KPIs, graphiques, fiscalité
│       ├── ClientService.cs     ← CRUD clients
│       ├── ExportService.cs     ← Génération CSV comptable
│       ├── FactureService.cs    ← Logique métier factures
│       ├── ParametreService.cs  ← Timbre fiscal, taux TVA
│       └── ProduitService.cs    ← CRUD produits + stock
│
└── Facturation.Web/
    ├── Components/
    │   ├── Layout/
    │   │   ├── MainLayout.razor   ← Layout principal (AppBar + Drawer)
    │   │   ├── NavMenu.razor      ← Menu de navigation latéral
    │   │   └── EmptyLayout.razor  ← Layout sans menu (page Login)
    │   └── Pages/
    │       ├── Home.razor                      ← Dashboard principal
    │       ├── Login.razor                     ← Page de connexion
    │       ├── Clients/
    │       │   ├── Index.razor                 ← Liste des clients
    │       │   └── Form.razor                  ← Formulaire ajout/édition
    │       ├── Factures/
    │       │   ├── Index.razor                 ← Liste des factures
    │       │   ├── Form.razor                  ← Création/édition facture
    │       │   └── Details.razor               ← Vue détaillée facture
    │       ├── Produits/
    │       │   ├── Index.razor                 ← Catalogue produits
    │       │   └── Form.razor                  ← Formulaire produit
    │       ├── TableauxDeBord/
    │       │   ├── Ventes.razor                ← Analytique ventes
    │       │   └── Fiscal.razor                ← Analytique fiscalité
    │       ├── Alertes/
    │       │   └── Stock.razor                 ← Alertes stock faible
    │       ├── Export/
    │       │   ├── ComptableCsv.razor          ← Export CSV comptable
    │       │   └── Previsions/Index.razor      ← Prévisions financières
    │       └── Parametres/
    │           └── Index.razor                 ← Paramètres système
    ├── appsettings.json           ← Configuration (DB, logging)
    └── Program.cs                 ← Injection de dépendances + middlewares
```

---

## 🗄️ Modèle de Données

### Diagramme des Entités

```
┌─────────────┐       ┌──────────────────┐       ┌─────────────────┐
│   Client    │       │     Facture      │       │   LigneFacture  │
│─────────────│       │──────────────────│       │─────────────────│
│ Id          │◄──────│ ClientId (FK)    │◄──────│ FactureId (FK)  │
│ Nom         │       │ NumeroFacture    │       │ ProduitId (FK)  │
│ Email       │       │ DateFacture      │       │ Quantite        │
│ Telephone   │       │ Statut           │       │ PrixUnitaireHT  │
│ Adresse     │       │ TotalHT          │       │ TauxTVA         │
│ MatriculeFi │       │ TotalTVA         │       │ MontantHT       │
│ DateCreatio │       │ TotalTTC         │       │ MontantTVA      │
└─────────────┘       │ TimbreFiscal     │       │ MontantTTC      │
                      │ MontantTotal     │       └────────┬────────┘
                      └──────────────────┘                │
                                                          │
┌─────────────┐                                  ┌────────▼────────┐
│  Parametre  │                                  │     Produit     │
│─────────────│                                  │─────────────────│
│ Cle         │                                  │ Id              │
│ Valeur      │                                  │ Code            │
└─────────────┘                                  │ Libelle         │
                                                  │ PrixUnitaireHT  │
                                                  │ TauxTVA         │
                                                  │ StockActuel     │
                                                  │ SeuilMinimal    │
                                                  │ EstActif        │
                                                  └─────────────────┘
```

### Taux de TVA Tunisiens Supportés

| Taux | Description |
|------|-------------|
| **0%** | Exonéré |
| **7%** | Taux réduit |
| **13%** | Taux intermédiaire |
| **19%** | Taux standard |

---

## 🧩 Modules Fonctionnels

### `AnalyseService`
Service d'agrégation des données pour les tableaux de bord.

- `GetDashboardDataAsync(debut, fin)` → KPIs + top clients + top produits
- `GetFiscalDashboardDataAsync(debut, fin)` → TVA collectée + timbres + évolution mensuelle
- `GetVentesDashboardDataAsync(debut, fin)` → CA par période + répartition

### `FactureService`
Logique métier complète pour la facturation.

- Génération automatique du numéro de facture (`FAC-XXXXXX`)
- Calcul automatique HT / TVA / TTC à la sauvegarde
- Ajout / suppression de lignes de facture
- Changement de statut (Brouillon → Validée → Annulée)

### `ExportService`
Génération des exports comptables.

- Export CSV des factures validées sur une période donnée
- Format compatible logiciels comptables (colonnes : Date, N° Facture, Client, HT, TVA, TTC, Timbre)

### `ProduitService`
Gestion du catalogue et des stocks.

- CRUD complet avec validation des codes uniques
- `GetProduitsEnAlerteStockAsync()` → produits dont `StockActuel < SeuilMinimal`

---

## 🎨 Composants UI

### Design System
L'application utilise un système de design cohérent basé sur :

- **Couleur principale** : `#1565C0` (bleu Delta)
- **Couleur secondaire** : `#00BFA5` (turquoise)
- **Police titre** : Poppins (Google Fonts)
- **Police texte** : Inter (Google Fonts)
- **Rayon des cartes** : `12px`

### Page de Connexion
Interface glassmorphisme avec :
- Fond animé (bulles flottantes)
- Logo animé avec effet pulse
- Champs MudBlazor avec icônes
- Accès rapide démonstration (boutons pré-remplissage)

### Navigation
- **AppBar** : Logo DeltaFacturation, notifications, paramètres, menu utilisateur
- **Drawer latéral** : Menu principal organisé par catégories (Ventes, Catalogue, Répertoire, Analytique, Outils)

### Graphiques (Radzen Blazor)
- `RadzenColumnSeries` : CA mensuel par barres
- `RadzenLineSeries` : Évolution temporelle
- `RadzenAreaSeries` : Surfaces d'évolution TVA
- `RadzenDonutSeries` : Répartition TVA par taux

> ⚠️ **Important** : `RadzenSeriesMeanLine` n'est compatible qu'avec les graphiques Cartésiens (Column, Line, Area). Ne pas l'utiliser dans un `RadzenDonutSeries` ou `RadzenPieSeries`.

---

## 🐛 Problèmes Connus & Solutions

### Port déjà utilisé au démarrage
Si vous obtenez `Address already in use` sur le port 5007 :

```powershell
# Windows PowerShell
Get-NetTCPConnection -LocalPort 5007 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

### Session perdue lors de la navigation
L'`IAuthenticationService` doit être enregistré en **Singleton** (non Scoped) dans `Program.cs` pour persister la session entre les pages Blazor Interactive Server.

### Render mode et interactivité
Toutes les pages utilisent `@rendermode InteractiveServer` défini globalement dans `App.razor` pour garantir un circuit SignalR stable sans recréation entre les navigations.

---

## 📄 Licence

Projet développé à des fins académiques et professionnelles.  
© 2026 DeltaFacturation — Tous droits réservés.
