terraform {
  backend "s3" {
    bucket       = "secret-nick"
    key          = "terraform.tfstate"
    region       = "eu-north-1"
    use_lockfile = true
    encrypt      = true
  }
}
