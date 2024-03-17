# Minecraft Launcher for Niwaki

## English

### Overview

This Minecraft Launcher, developed in .NET CORE 8, is tailored for the Niwaki Minecraft server. It fetches version information from [the official Niwaki website](https://niwaki-mc.fr), allows for the installation of predefined mods through [Modrinth API](https://modrinth.com) integration, and features an auto-update system powered by [NetSparkle](https://netsparkleupdater.github.io/NetSparkle/).

### Features

- Fetches version information from Niwaki's official site.
- Installs predefined mods suitable for the Niwaki server via Modrinth API.
- Auto-update system for seamless software updates.

### Dependencies

Dependencies are organized into categories for ease of understanding and each comes with a brief description:

<table>
  <thead>
    <tr>
      <th>Category</th>
      <th>Package</th>
      <th>Description</th>
      <th>Attribution Link</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td rowspan="6">UI and Theming</td>
      <td>Avalonia 11.0.10</td>
      <td>Provides the UI framework for the launcher.</td>
      <td rowspan="6"><a href="https://avaloniaui.net">Avalonia</a></td>
    </tr>
    <tr>
      <td>Avalonia.Desktop 11.0.10</td>
      <td>Enables desktop application capabilities for Avalonia.</td>
    </tr>
    <tr>
      <td>Avalonia.Fonts.Inter 11.0.10</td>
      <td>Provides font support for Avalonia applications.</td>
    </tr>
    <tr>
      <td>Avalonia.ReactiveUI 11.0.10</td>
      <td>Integrates ReactiveUI with Avalonia for reactive programming.</td>
    </tr>
    <tr>
      <td>Avalonia.Themes.Fluent 11.0.10</td>
      <td>Offers Fluent Design themes for Avalonia applications.</td>
    </tr>
    <tr>
      <td>MessageBox.Avalonia 3.1.5.1</td>
      <td>Displays message boxes and notifications in Avalonia applications.</td>
    </tr>
    <tr>
      <td>Auto-Update</td>
      <td>NetSparkleUpdater.UI.Avalonia 3.0.0-preview20240219001</td>
      <td>Facilitates automatic updates for the launcher UI built with Avalonia.</td>
      <td><a href="https://netsparkleupdater.github.io/NetSparkle/">NetSparkle</a></td>
    </tr>
    <tr>
      <td rowspan="2">Minecraft Integration</td>
      <td>CmlLib.Core 3.3.10</td>
      <td>Provides core functionalities for integrating with Minecraft.</td>
      <td rowspan="2"><a href="https://alphabs.gitbook.io/cmllib/cmllib.core/cmllib.core">CmlLib</a></td>
    </tr>
    <tr>
      <td>CmlLib.Core.Auth.Microsoft 3.0.2</td>
      <td>Handles Microsoft authentication for Minecraft accounts.</td>
    </tr>
    <tr>
      <td rowspan="2">System Management</td>
      <td>System.Management 9.0.0-preview.1.2.4080.9</td>
      <td>Allows for retrieving system information, such as RAM, within the launcher.</td>
      <td rowspan="2">N/A</td>
    </tr>
    <tr>
      <td>PresentationFramework 4.6.0</td>
      <td>Offers presentation and UI capabilities for .NET applications, including screen resolution.</td>
    </tr>
  </tbody>
</table>

## French

### Vue d'ensemble

Ce Launcher Minecraft, développé en .NET CORE 8, est conçu sur mesure pour le serveur Minecraft Niwaki. Il récupère les informations de versions depuis le [site officiel de Niwaki](https://niwaki-mc.fr), permet l'installation de mods pré-définis grâce à l'intégration de l'[API Modrinth](https://modrinth.com), et dispose d'un système d'auto-update propulsé par [NetSparkle](https://netsparkleupdater.github.io/NetSparkle/).

### Fonctionnalités

- Récupère les informations de versions depuis le site officiel de Niwaki.
- Installe des mods pré-définis adaptés au serveur Niwaki via l'API Modrinth.
- Système d'auto-update pour des mises à jour logicielles fluides.

### Dépendances

Les dépendances sont organisées en catégories pour faciliter la compréhension et chaque paquet vient avec une brève description :


<table>
  <thead>
    <tr>
      <th>Catégorie</th>
      <th>Paquet</th>
      <th>Description</th>
      <th>Lien d'attribution</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td rowspan="6">UI et Thématisation</td>
      <td>Avalonia 11.0.10</td>
      <td>Fournit le cadre UI pour le launcher.</td>
      <td rowspan="6"><a href="https://avaloniaui.net">Avalonia</a></td>
    </tr>
    <tr>
      <td>Avalonia.Desktop 11.0.10</td>
      <td>Permet les capacités d'application de bureau pour Avalonia.</td>
    </tr>
    <tr>
      <td>Avalonia.Fonts.Inter 11.0.10</td>
      <td>Fournit le support des polices pour les applications Avalonia.</td>
    </tr>
    <tr>
      <td>Avalonia.ReactiveUI 11.0.10</td>
      <td>Intègre ReactiveUI avec Avalonia pour la programmation réactive.</td>
    </tr>
    <tr>
      <td>Avalonia.Themes.Fluent 11.0.10</td>
      <td>Propose des thèmes Fluent Design pour les applications Avalonia.</td>
    </tr>
    <tr>
      <td>MessageBox.Avalonia 3.1.5.1</td>
      <td>Affiche des boîtes de dialogue et des notifications dans les applications Avalonia.</td>
    </tr>
    <tr>
      <td>Mises à jour automatiques</td>
      <td>NetSparkleUpdater.UI.Avalonia 3.0.0-preview20240219001</td>
      <td>Facilite les mises à jour automatiques pour l'interface utilisateur du launcher construite avec Avalonia.</td>
      <td><a href="https://netsparkleupdater.github.io/NetSparkle/">NetSparkle</a></td>
    </tr>
    <tr>
      <td rowspan="2">Intégration Minecraft</td>
      <td>CmlLib.Core 3.3.10</td>
      <td>Fournit des fonctionnalités de base pour l'intégration avec Minecraft.</td>
      <td rowspan="2"><a href="https://alphabs.gitbook.io/cmllib/cmllib.core/cmllib.core">CmlLib</a></td>
    </tr>
    <tr>
      <td>CmlLib.Core.Auth.Microsoft 3.0.2</td>
      <td>Gère l'authentification Microsoft pour les comptes Minecraft.</td>
    </tr>
    <tr>
      <td rowspan="2">Gestion du Système</td><td>System.Management 9.0.0-preview.1.2.4080.9</td>
      <td>Permet de récupérer des informations système, telles que la RAM, dans le launcher.</td>
      <td rowspan="2">N/A</td>
    </tr>
    <tr>
      <td>PresentationFramework 4.6.0</td>
      <td>Offre des capacités de présentation et d'UI pour les applications .NET, y compris la résolution de l'écran.</td>
    </tr>
  </tbody>
</table>

