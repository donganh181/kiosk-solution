def remote = [:]
remote.name = 'remote-server'
remote.host = '172.168.2.5'
remote.port = 22
remote.user = 'root'
remote.password = 'Goboi123'
remote.allowAnyHosts = true

pipeline {
    agent any
    tools {
        dockerTool 'docker'
    }
    stages {
        // stage('Cloning git') {
        //     steps {
        //         git url: 'https://github.com/donganh181/kiosk-solution', branch: 'production', credentialsId: 'github'
        //     }
        // }
        // stage('Build Image') {
        //     steps {
        //         sh "docker build -t longpc/kiosk-solution ."
        //     }
        // }
        // stage('Test') {
        //     steps {
        //         echo 'Testing..'
        //     }
        // }
        // stage("Login dockerhub") {
        //     steps {
        //        withCredentials([usernamePassword(credentialsId: 'docker-hub', passwordVariable: 'pass', usernameVariable: 'user')]) {
        //             sh "docker login -u $user -p $pass"
        //        }
        //     }
        // }
        // stage("Push Image") {
        //     steps {
        //        sh "docker push longpc/kiosk-solution"
        //     }
        // }
        stage('Remote SSH') {
            steps {
                sshCommand remote: remote, command: "docker pull longpc/kiosk-solution"
                sshCommand remote: remote, command: "cd /opt/capstone && docker-compose down --rmi all"
                sshCommand remote: remote, command: "cd /opt/capstone && docker-compose up -d"
            }
        }
    }
}