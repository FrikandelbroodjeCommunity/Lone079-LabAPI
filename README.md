[![GitHub release](https://flat.badgen.net/github/release/FrikandelbroodjeCommunity/Lone079-LabAPI/)](https://github.com/FrikandelbroodjeCommunity/Lone079-LabAPI/releases/latest)
[![LabAPI Version](https://flat.badgen.net/static/LabAPI%20Version/v1.1.4)](https://github.com/northwood-studios/LabAPI)
[![Original](https://flat.badgen.net/static/Original/NaxefirYT?icon=github)](https://github.com/NaxefirYT/Lone079Rework)
[![License](https://flat.badgen.net/github/license/FrikandelbroodjeCommunity/Lone079-LabAPI/)](https://github.com/FrikandelbroodjeCommunity/Lone079-LabAPI/blob/main/LICENSE)

# About Lone079

Lone079is a LabAPI plugin for SCP: Secret Laboratory that transforms SCP-079 into a random SCP if it becomes the last
SCP alive. After transformation, SCP-079 spawns in the containment chamber of the selected SCP.

> [!NOTE]
> This plugin is a port of a rework of the original [Lone079](https://github.com/Cyanox62/Lone079) by Cyanox62.

# Installation

Place the [latest release](https://github.com/FrikandelbroodjeCommunity/Lone079-LabAPI/releases/latest) in
the LabAPI plugin folder.

# Config

| Config                      | Default | Meaning                                                                                                                  |
|-----------------------------|---------|--------------------------------------------------------------------------------------------------------------------------|
| `debug`                     | `false` | When enabled, the plugin will show debug message. When using on a public server it is recommended to keep this disabled. |
| `count_zombies`             | `false` | Whether SCP-049-2 instances should be seen as SCPs. When set to false, SCP-079 can be replaced while there are zombies.  |
| `scale_with_level`          | `false` | When enabled, the amount of health SCP-079 gets when respawning is dependend on their level.                             |
| `health_percent`            | `50`    | How many % of health SCP-079 should be revived with. (0-100)                                                             |
| `broadcast_message`         | ...     | Message shown to SCP-079 when being revived.                                                                             |
| `broadcast_duration`        | `10`    | Duration in seconds the message `broadcast_message` is shown.                                                            |
| `public_broadcast_message`  | ...     | Message shown to all alive players when SCP-079 is revived.                                                              |
| `public_broadcast_duration` | `10`    | Duration in seconds the message `public_broadcast_message` is shown.                                                     |
| `respawn_delay`             | `1`     | Delay before respawning SCP-079 after it is the last SCP alive.                                                          |
| `transform_on_recontain`    | `false` | Whether SCP-079 should be revived when recontained.                                                                      |
| `scp079_available_roles`    | ...     | The roles SCP-079 can become when being revived.                                                                         |
