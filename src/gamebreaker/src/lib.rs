use std::io::Read;

#[cfg(target_os = "windows")]
mod win;

pub fn main() {
    let mut config_file = std::fs::File::open("gamebreaker.toml").unwrap();
    let mut config = String::new();
    config_file.read_to_string(&mut config).unwrap();

    let config: toml::Value = toml::from_str(&config).unwrap();
}
