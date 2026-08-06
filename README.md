# Beyond The Line

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![License: CC BY-NC 4.0](https://img.shields.io/badge/License-CC_BY--NC_4.0-lightgrey.svg)](https://creativecommons.org/licenses/by-nc/4.0/)

Welcome to the source code repository for "Beyond The Line", a text-based serious game developed in Unity, C#, and Ink.

The official Master's thesis document is available in the repository releases.

👉 **Please visit the [Releases](https://github.com/giulio-arecco/beyond-the-line/releases) section to download the Thesis PDF.**

The document provides a high-level overview of the project's key technical and theoretical foundations, including:
* The game's modular architecture and custom state management.
* The integration of the Ink narrative engine within Unity.
* The custom runtime telemetry system for data collection.
* The application of the Complex Problem Solving framework.

## License

This project uses a dual-license structure to separate the software infrastructure from the creative narrative content:

* **Source Code:** All C# scripts, engine configurations, and source code are licensed under the [MIT License](LICENSE).
* **Narrative Assets:** The story, characters, dialogues, and Ink script files are licensed under the [Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)](Assets/Story/LICENSE-NARRATIVE).

You are free to use, modify, and build upon the source code for any purpose, provided you include the original copyright notice. However, the narrative content of *Beyond The Line* cannot be used for commercial purposes without explicit permission.

## Dependencies & Omitted Assets

While this repository contains the complete custom source code and the original narrative framework developed for the thesis, **all graphical, audio, and proprietary third-party assets have been intentionally excluded.** This decision was made to comply with third-party EULAs and to avoid licensing ambiguities regarding AI-generated media.

Because these assets and references are omitted, **the project is not fully playable or operational within the Unity Editor** directly from a clone. 

### Third-Party Plugins
If you wish to inspect the project structure in the Editor, please note that the original development relied on the following third-party plugins:

* [**NodeCanvas**](https://assetstore.unity.com/packages/tools/visual-scripting/nodecanvas-14914) by *ParadoxNotion* (Expected path: `Assets/ParadoxNotion/`)
* [**Scene Attribute**](https://assetstore.unity.com/packages/tools/utilities/scene-attribute-reference-scenes-in-inspector-316227) by *Agent40* (Expected path: `Assets/SceneAttribute/`)
* [**UltEvents**](https://assetstore.unity.com/packages/tools/gui/ultevents-111307) by *Kybernetik* (Expected path: `Packages/com.kybernetik.ultevents/`)
* [**DOTween Pro**](https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416) by *Demigiant* (Expected path: `Assets/Plugins/Demigiant/`)

👉 **To experience the game, please download the compiled executable available in the [Releases](https://github.com/giulio-arecco/beyond-the-line/releases) section.**
